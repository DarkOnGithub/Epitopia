using System.Collections.Generic;

namespace Event.Core
{
    public class Event
    {
        public static List<EventListener> Listeners = new();
        public bool IsCancelled = false;
        public Event() {}
        
    }
}