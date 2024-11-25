using System;

namespace Events.EventHandler
{
    public class DefaultListener : Listener
    {
        public DefaultListener(Action<IEvent> action)
        {
            Action = action;
        }
        
    }
}