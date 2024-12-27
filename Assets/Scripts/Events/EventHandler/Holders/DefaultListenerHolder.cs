using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Events.EventHandler.Holders
{
    public class DefaultListenerHolder : IListenersHolder
    {
        protected readonly Dictionary<EventPriority, HashSet<Listener>> Listeners = new();

        public DefaultListenerHolder()
        {
            foreach (var priority in EventPriorityHelper.GetPriorities)
                Listeners[priority] = new HashSet<Listener>();
        }

        public void AddListener(Listener listener)
        {
            if (listener == null)
                throw new ArgumentNullException(nameof(listener));

            if (!Listeners.TryGetValue(listener.Priority, out var listenerSet))
                throw new ArgumentException($"Invalid event priority: {listener.Priority}");
            listenerSet.Add(listener);
        }

        public void RemoveListener(Action<IEvent> listener)
        {
            if (listener == null)
                throw new ArgumentNullException(nameof(listener));

            foreach (var listenerSet in Listeners.Values)
            {
                var listenerToRemove = listenerSet.FirstOrDefault(l => l.Action == listener);
                if (listenerToRemove != null)
                {
                    listenerSet.Remove(listenerToRemove);
                    break;
                }
            }
        }

        public IEnumerable<Listener> GetListeners()
        {
            return Listeners
                  .OrderBy(kvp => kvp.Key)
                  .SelectMany(kvp => kvp.Value);
        }

        public IEnumerable<Listener> GetListenersByPriority(EventPriority priority)
        {
            if (!Listeners.TryGetValue(priority, out var listenerSet))
                throw new ArgumentException($"Invalid event priority: {priority}");
            return listenerSet.ToList();
        }

        public bool Invoke(IEvent @event)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            var listeners = GetListeners().ToList();

            foreach (var listener in listeners)
            {
                try
                {
                    listener.Action?.Invoke(@event);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning(ex);
                    continue;
                }

                if (listener.IsWeak)
                    Listeners[listener.Priority].Remove(listener);
                if (@event.IsCancelled)
                    return true;
            }

            return false;
        }
    }
}