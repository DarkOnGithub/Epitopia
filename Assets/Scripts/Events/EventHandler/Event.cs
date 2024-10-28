using System;
using System.Collections.Generic;

namespace Events.EventHandler
{
    public abstract class Event
    {
        public List<EventListener> GetListeners => EventFactory.ListenersContainer[this.GetType()];
        public bool IsCancelled = false;
        public Event() {}
        
    }
}