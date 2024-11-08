using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Events.EventHandler
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SubscribeEventAttribute : Attribute
    {
        public EventPriority Priority = EventPriority.Normal;

        public static IEnumerable<MethodInfo> GetMethods(object type)
        {
            return (type is Type ? (Type)type : type.GetType())
                  .GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
                              BindingFlags.Instance | BindingFlags.Static)
                  .Where(method => method.GetCustomAttributes(typeof(SubscribeEventAttribute), false).Any());
        }
    }
}