using System;

namespace Events.EventHandler
{
    public abstract class Listener
    {
        public Action<IEvent> Action;
        public bool IsWeak;
        public EventPriority Priority = EventPriority.Normal;

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