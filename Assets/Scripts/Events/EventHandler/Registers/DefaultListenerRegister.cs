using System;

namespace Events.EventHandler.Registers
{
    public class DefaultListenerRegister<T> : ListenerRegister<T> where T : IEvent
    {
        public DefaultListenerRegister(IListenersHolder holder) : base(holder)
        {
        }

        public DefaultListenerRegister<T> Register(Action<T> listener, EventPriority priority = EventPriority.Normal)
        {
            Holder.AddListener(new DefaultListener(e => listener((T)e))
                                  .WithPriority(priority));
            return this;
        }

        public DefaultListenerRegister<T> Unregister(Action<IEvent> listener)
        {
            Holder.RemoveListener(listener);
            return this;
        }

        public DefaultListenerRegister<T> RegisterOnce(Action<IEvent> listener,
            EventPriority priority = EventPriority.Normal)
        {
            Holder.AddListener(new DefaultListener(e => listener((T)e))
                              .WithPriority(priority)
                              .AsWeak(true));

            return this;
        }


        public static DefaultListenerRegister<T> operator -(DefaultListenerRegister<T> listenerRegister,
            Action<IEvent> listener)
        {
            return listenerRegister.Unregister(listener);
        }

        public static DefaultListenerRegister<T> operator +(DefaultListenerRegister<T> listenerRegister,
            Action<T> listener)
        {
            return listenerRegister.Register(listener);
        }
    }
}