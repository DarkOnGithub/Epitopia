using System.Collections.Generic;
using Events.EventHandler;

namespace Events
{
    public class SampleEvent : Event, ICancellable
    {
        private new static List<EventListener> Listeners { get; } = new();
        public string s;
        public int i;
        public SampleEvent(string s1, int i1) 
        {
            s = s1;
            i = i1;
        }
    }
}