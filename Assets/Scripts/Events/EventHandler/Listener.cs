using System;

namespace Events.EventHandler
{
    public abstract class Listener
    {
        public Action<IEvent> Action;
        public EventPriority Priority = EventPriority.Normal;
        public bool IsWeak = false;
        
        public Listener AsWeak(bool weak)
        {
            IsWeak = weak;
            return this;
        }
        
        public Listener WithPriority(EventPriority priority)
        {
            Priority = priority;
            return this;
        }
    }
}