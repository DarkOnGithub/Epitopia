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
        private static List<EventListener> GetListener(Type type) => ListenersContainer[type]; 
        private static IEnumerable<Type> SearchForEventClasses()
        {
            var eventType = typeof(Event);
            var assembly = eventType.Assembly;

            return assembly.GetTypes().Where(t => t.IsSubclassOf(eventType) && !ListenersContainer.ContainsKey(t));
        }

        public static void RegisterEvents()
        {
            foreach (var eventClass in SearchForEventClasses())
            {
                ListenersContainer.TryAdd(eventClass, new());
            }
        }
        /// <summary>
        /// Register a class to the event system
        /// </summary>
        /// <param name="T">The instance that is registered</param>
        /// <warning>Make sure that the class has the SubscribeEvent attribute</warning>
        public static void Register(object T)
        {
            Type type = T as Type ?? T.GetType();
            bool isStatic = type == (T as Type);
            Debug.Log("a");

            foreach (var method in SubscribeEventAttribute.GetMethods(T))
            {
                var attribute = method.GetCustomAttribute<SubscribeEventAttribute>(true);
                if (attribute == null)
                {
                    continue;
                }
                
                var parameters = method.GetParameters();
                if(parameters.Length != 1)
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
            var priority = attribute.Priority;
            var listener = new EventListener(isStatic || method.IsStatic ? null : instance, method, priority);
            var listeners = GetListener(eventType);
            if (listeners == null)
            {
                _logger.LogWarning($"Event <{eventType.Name}> isn't initialized");
                return;
            }
            switch (priority)
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
                Debug.Log(listener.Priority);
                listener.Invoke(evt);
            }
        }
    }
}