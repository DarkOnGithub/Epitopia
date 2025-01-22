using System;


namespace Events.EventHandler.Registers
{
    public class ConditionalListenerRegister<T, TT> : ListenerRegister<T> where T : IEvent
    {
        public ConditionalListenerRegister(IListenersHolder holder) : base(holder)
        {
        }

        public ConditionalListenerRegister<T, TT> RegisterWhen(Action<T> listener,
            TT condition,
            EventPriority priority = EventPriority.Normal)
        {
            Holder.AddListener(new ConditionalListener<TT>()
                               {
                                   Condition = condition,
                                   Action = e => listener((T)e)
                               }.WithPriority(priority));

            return this;
        }

        public ConditionalListenerRegister<T, TT> RegisterOnce(Action<T> listener,
            TT condition,
            EventPriority priority = EventPriority.Normal)
        {
            Holder.AddListener(new ConditionalListener<TT>()
                               {
                                   Condition = condition,
                                   Action = e => listener((T)e)
                               }.WithPriority(priority).AsWeak(true));

            return this;
        }

        public ConditionalListenerRegister<T, TT> Unregister(Action<IEvent> listener)
        {
            Holder.RemoveListener(listener);
            return this;
        }

        public static ConditionalListenerRegister<T, TT> operator -(ConditionalListenerRegister<T, TT> register,
            Action<IEvent> listener)
        {
            return register.Unregister(listener);
        }

        public static ConditionalListenerRegister<T, TT> operator +(ConditionalListenerRegister<T, TT> register,
            (Action<T> listener, TT condition) registration)
        {
            return register.RegisterWhen(registration.listener, registration.condition);
        }
    }
}