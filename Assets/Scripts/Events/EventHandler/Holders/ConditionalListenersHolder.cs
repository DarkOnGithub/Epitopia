using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Events.EventHandler.Holders
{
    public class ConditionalListenersHolder<T> : IListenersHolder
    {
        private readonly Dictionary<EventPriority, Dictionary<T, HashSet<Listener>>> _listeners;

        public ConditionalListenersHolder()
        {
            _listeners = new Dictionary<EventPriority, Dictionary<T, HashSet<Listener>>>();
            
            foreach (var priority in EventPriorityHelper.GetPriorities)
                _listeners[priority] = new Dictionary<T, HashSet<Listener>>();
            
        }

        public void AddListener(Listener listener)
        {
            if (listener == null)
                throw new ArgumentNullException(nameof(listener));

            if (listener is not ConditionalListener<T> conditionalListener)
                throw new ArgumentException($"Listener must be of type ConditionalListener<{typeof(T).Name}>");

            var priorityTable = _listeners[listener.Priority];
            
            if (!priorityTable.TryGetValue(conditionalListener.Condition, out var listeners))
            {
                listeners = new HashSet<Listener>();
                priorityTable[conditionalListener.Condition] = listeners;
            }
            
            listeners.Add(listener);
        }

        public void RemoveListener(Action<IEvent> listener)
        {
            if (listener == null)
                throw new ArgumentNullException(nameof(listener));

            if (listener is not ConditionalListener<T> conditionalListener)
                throw new ArgumentException($"Listener must be of type ConditionalListener<{typeof(T).Name}>");

            foreach (var priorityTable in _listeners.Values)
            {
                if (priorityTable.TryGetValue(conditionalListener.Condition, out var listeners))
                {
                    var listenerToRemove = listeners.FirstOrDefault(l => l.Action == (Action<IEvent>)(object)listener);
                    if (listenerToRemove != null)
                    {
                        listeners.Remove(listenerToRemove);
                        break;
                    }
                }
            }
        }

        public IEnumerable<Listener> GetListeners()
        {
            return _listeners
                .SelectMany(priorityEntry => priorityEntry.Value.Values)
                .SelectMany(listeners => listeners);
        }

        public IEnumerable<Listener> GetListenersByPriority(EventPriority priority)
        {
            if (!_listeners.TryGetValue(priority, out var priorityTable))
                return Enumerable.Empty<Listener>();

            return priorityTable.Values.SelectMany(listeners => listeners);
        }

        public IEnumerable<Listener> GetListenersByCondition(T condition)
        {
            return _listeners
                .Select(priorityEntry => priorityEntry.Value)
                .Where(priorityTable => priorityTable.ContainsKey(condition))
                .Select(priorityTable => priorityTable[condition])
                .SelectMany(listeners => listeners);
        }
        
        public bool Invoke(IEvent @event, T condition)
        {
            foreach (var listener in GetListenersByCondition(condition))
            {
                try
                {
                    listener.Action.Invoke(@event);
                }catch (Exception ex)
                {
                    Debug.LogWarning(ex);
                    continue;
                }
                if(listener.IsWeak)
                    _listeners[listener.Priority][condition].Remove(listener);
                if (@event.IsCancelled)
                    return true;
            }
            return false;
        }
    }
}