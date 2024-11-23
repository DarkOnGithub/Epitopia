using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core;
using JetBrains.Annotations;

namespace Events.EventHandler
{
    public static class EventFactory
    {
        private static readonly ConcurrentDictionary<Type, List<EventListener>> ListenersContainer = new();
        private static readonly BetterLogger Logger = new(typeof(EventFactory));

        public static List<EventListener> GetListener(Type type) => 
            ListenersContainer.GetValueOrDefault(type);

        private static IEnumerable<Type> SearchForEventClasses()
        {
            var eventType = typeof(Event);
            return eventType.Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(eventType) && !ListenersContainer.ContainsKey(t));
        }

        public static void RegisterEvents()
        {
            foreach (var eventClass in SearchForEventClasses())
            {
                ListenersContainer.TryAdd(eventClass, new List<EventListener>());
            }
        }

        public static void Register([NotNull] object target)
        {
            var type = target as Type ?? target.GetType();
            var isStatic = target is Type;

            foreach (var method in SubscribeEventAttribute.GetMethods(target))
            {
                var attribute = method.GetCustomAttribute<SubscribeEventAttribute>(true);
                if (attribute == null) continue;

                if (!ValidateMethod(method, type)) continue;

                var eventType = method.GetParameters()[0].ParameterType;
                Subscribe(eventType, method, attribute, target, isStatic);
            }
        }

        private static bool ValidateMethod(MethodInfo method, Type type)
        {
            var parameters = method.GetParameters();
            if (parameters.Length != 1)
            {
                Logger.LogWarning($"[Skipping] Method '{method.Name}' in type '{type.Name}' must have exactly one parameter");
                return false;
            }

            var eventType = parameters[0].ParameterType;
            if (!typeof(Event).IsAssignableFrom(eventType))
            {
                Logger.LogWarning($"[Skipping] Method '{method.Name}' in type '{type.Name}' must have a parameter that inherits from Event");
                return false;
            }

            return true;
        }

        public static void Subscribe(
            [NotNull] Type eventType,
            [NotNull] MethodInfo method,
            [NotNull] SubscribeEventAttribute attribute,
            [CanBeNull] object instance,
            bool isStatic = false)
        {
            if (!ListenersContainer.TryGetValue(eventType, out var listeners))
            {
                Logger.LogWarning($"Event '{eventType.Name}' isn't initialized");
                return;
            }

            var listener = new EventListener(isStatic || method.IsStatic ? null : instance, method, attribute.Priority);

            lock (listeners)
            {
                InsertListener(listeners, listener, attribute.Priority);
            }
        }

        private static void InsertListener(List<EventListener> listeners, EventListener listener, EventPriority priority)
        {
            switch (priority)
            {
                case EventPriority.High:
                    listeners.Insert(0, listener);
                    break;
                case EventPriority.Normal:
                    var index = listeners.FindLastIndex(l => l.Priority == EventPriority.High);
                    listeners.Insert(index + 1, listener);
                    break;
                case EventPriority.Low:
                    listeners.Add(listener);
                    break;
            }
        }

        public static void Invoke([NotNull] Event evt)
        {
            var listeners = GetListener(evt.GetType());
            if (listeners == null)
            {
                Logger.LogWarning($"Event '{evt.GetType().Name}' isn't initialized");
                return;
            }

            // Create a snapshot of listeners to prevent modification during iteration
            var snapshot = listeners.ToArray();
            foreach (var listener in snapshot)
            {
                if (evt.IsCancelled) break;
                try
                {
                    listener.Invoke(evt);
                }
                catch (Exception ex)
                {
                    Logger.LogWarning($"Error invoking event listener: {ex.Message}");
                }
            }
        }
    }
}