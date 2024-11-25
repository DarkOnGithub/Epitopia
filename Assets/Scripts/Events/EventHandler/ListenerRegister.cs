
using System;

namespace Events.EventHandler
{
    public abstract class ListenerRegister<T> where T : IEvent
    {
        protected readonly IListenersHolder Holder;

        public ListenerRegister(IListenersHolder holder)
        {
            Holder = holder;
        }
    }
}