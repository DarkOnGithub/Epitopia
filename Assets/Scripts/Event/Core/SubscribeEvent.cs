using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Event.Core
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class SubscribeEventAttribute : Attribute
    {
        public EventPriority Priority = EventPriority.Normal;
        /// <summary>
        /// Gets all methods that have a specific attribute from a type
        /// </summary>
        /// <param name="type">The type to inspect</param>
        /// <returns>List of MethodInfo objects that have the specified attribute</returns>
        public static IEnumerable<MethodInfo> GetMethods(object type)
        {
            return type.GetType()
                  .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | 
                              BindingFlags.Instance | BindingFlags.Static)
                  .Where(method => method.GetCustomAttributes(typeof(SubscribeEventAttribute), false).Any());
        }
    }
}