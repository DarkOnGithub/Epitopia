using System;
using System.Collections.Generic;

namespace Old
{
    public abstract class Event
    {
        public List<EventListener> GetListeners => EventFactory.GetListener(GetType());
        public bool IsCancelled = false;

        public Event()
        {
        }

        public void SetCancelled(bool _)
        {
        }
    }
}