using System.Reflection;
using JetBrains.Annotations;

namespace Events.EventHandler
{
    public class EventListener
    {
        private MethodInfo _method;
        [CanBeNull] private object _instance;
        public EventPriority Priority { get; private set; }

        public EventListener([CanBeNull] object instance, MethodInfo method, EventPriority priority)
        {
            _method = method;
            _instance = instance;
            Priority = priority;
        }

        public void Invoke(Event evt)
        {
            _method.Invoke(_instance, new []{ evt });
        }
    }
}