using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core;
using UnityEngine;

namespace Event.Core
{
    public static class EventFactory
    {
        private static LogManager _logger = new(typeof(EventFactory));
        
        /// <summary>
        /// Register a class to the event system
        /// </summary>
        /// <param name="T">The instance that is registered</param>
        /// <warning>Make sure that the class has the SubscribeEvent attribute</warning>
        public static void Register(object T)
        {
            var type = T.GetType();
            var attributes = Attribute.GetCustomAttributes(type, typeof(SubscribeEventAttribute));
            
            if (attributes.Length == 0)  
            {
                _logger.LogWarning($"type <{type.Name}> doesn't contain any listener");
                return;
            }

            foreach (var method in SubscribeEventAttribute.GetMethods(T))
            {
                var parameters = method.GetParameters();
                if(parameters.Length != 1)
                {
                    _logger.LogError($"Method <{method.Name}> in type <{type.Name}> must have only one parameter");
                    return;
                }
                var eventType = parameters[0].ParameterType;
                if (!typeof(Event).IsAssignableFrom(eventType))
                {
                    _logger.LogError($"Method <{method.Name}> in type <{type.Name}> must have a parameter that inherits from Event");
                    return;
                }
                var priority = method.GetCustomAttribute<SubscribeEventAttribute>(true).Priority;

                var listener = new EventListener(T, method, priority);
                ((List<EventListener>)eventType.GetField("Listeners").GetValue(null)).Add(listener);                
            }
        }
        
        /// <summary>
        /// Register a class to the event system
        /// </summary>
        /// <typeparam name="T">The instance that is registered</typeparam>
        /// <warning>Make sure that the class has the SubscribeEvent attribute</warning>
        public static void Register<T>()
        {
            throw new NotImplementedException();
            // var type = typeof(T);
            // var attributes = Attribute.GetCustomAttributes(type, typeof(SubscribeEventAttribute));
            //
            // if (attributes.Length == 0)  
            // {
            //     _logger.LogWarning($"type <{type.Name}> doesn't contain any listener");
            //     return;
            // }
            //
            // foreach (var method in SubscribeEventAttribute.GetMethods(T))
            // {
            //     var parameters = method.GetParameters();
            //     if(parameters.Length != 1)
            //     {
            //         _logger.LogError($"Method <{method.Name}> in type <{type.Name}> must have only one parameter");
            //         return;
            //     }
            //     var eventType = parameters[0].ParameterType;
            //     if (!typeof(Event).IsAssignableFrom(eventType))
            //     {
            //         _logger.LogError($"Method <{method.Name}> in type <{type.Name}> must have a parameter that inherits from Event");
            //         return;
            //     }
            //     var priority = method.GetCustomAttribute<SubscribeEventAttribute>(true).Priority;
            //
            //     var listener = new EventListener(T, method, priority);
            //     ((List<EventListener>)eventType.GetField("Listeners").GetValue(null)).Add(listener);                
            // }
        }
    }
}