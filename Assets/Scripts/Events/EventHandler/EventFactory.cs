using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;

namespace Events.EventHandler
{
    public static class EventFactory
    {
             public static Dictionary<Type, List<EventListener>> ListenersContainer = new();
        private static BetterLogger _logger = new(typeof(EventFactory));

        private static List<EventListener> GetListener(Type type) => 
            ListenersContainer.TryGetValue(type, out var listeners) ? listeners : null;

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

        public static void Register(object T)
        {
            var type = T as Type ?? T.GetType();
            var isStatic = type == (T as Type);

            foreach (var method in SubscribeEventAttribute.GetMethods(T))
            {
                var attribute = method.GetCustomAttribute<SubscribeEventAttribute>(true);
                if (attribute == null) continue;

                var parameters = method.GetParameters();
                if (parameters.Length != 1)
                {
                    _logger.LogWarning($"[Skipping] Method <{method.Name}> in type <{type.Name}> must have only one parameter");
                    continue;
                }

                var eventType = parameters[0].ParameterType;
                if (!typeof(Event).IsAssignableFrom(eventType))
                {
                    _logger.LogWarning($"[Skipping] Method <{method.Name}> in type <{type.Name}> must have a parameter that inherits from Event");
                    return;
                }

                Subscribe(eventType, method, attribute, T, isStatic);
            }
        }

        public static void Subscribe(Type eventType, MethodInfo method, SubscribeEventAttribute attribute, [CanBeNull] object instance, bool isStatic = false)
        {
            var listener = new EventListener(isStatic || method.IsStatic ? null : instance, method, attribute.Priority);
            var listeners = GetListener(eventType);
            if (listeners == null)
            {
                _logger.LogWarning($"Event <{eventType.Name}> isn't initialized");
                return;
            }

            switch (attribute.Priority)
            {
                case EventPriority.High:
                    listeners.Insert(0, listener);
                    break;
                case EventPriority.Normal:
                    var index = listeners.FindLastIndex(evtListener => evtListener.Priority == EventPriority.High);
                    listeners.Insert(index + 1, listener);
                    break;
                case EventPriority.Low:
                    listeners.Add(listener);
                    break;
            }
        }

        public static void Invoke(Event evt)
        {
            var listeners = evt.GetListeners;
            if (listeners == null)
            {
                _logger.LogWarning($"Event <{evt.GetType().Name}> isn't initialized");
                return;
            }

            foreach (var listener in listeners)
            {
                if (evt.IsCancelled) break;
                listener.Invoke(evt);
            }
        }
    }
}