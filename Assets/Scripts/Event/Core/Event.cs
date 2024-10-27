using System;
using System.Collections.Generic;

namespace Event.Core
{
    public abstract class Event
    {
        public static List<EventListener> Listeners { get; } = new();
        public abstract List<EventListener> GetListeners();
        public bool IsCancelled = false;
        public Event() {}
        
    }
}