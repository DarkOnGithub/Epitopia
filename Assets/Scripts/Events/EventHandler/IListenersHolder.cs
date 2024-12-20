using System;
using System.Collections;
using System.Collections.Generic;

namespace Events.EventHandler
{
    public interface IListenersHolder
    {
        public void AddListener(Listener listener);

        public void RemoveListener(Action<IEvent> listener);

        public IEnumerable<Listener> GetListenersByPriority(EventPriority priority);
        public IEnumerable<Listener> GetListeners();
    }
}