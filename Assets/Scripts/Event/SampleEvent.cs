using System.Collections.Generic;
using Event.Core;

namespace Event
{
    public class SampleEvent : Core.Event, ICancellable
    {
        private new static List<EventListener> Listeners { get; } = new();
        public override List<EventListener> GetListeners() => Listeners;
        public string s;
        public int i;
        public SampleEvent(string s1, int i1) 
        {
            s = s1;
            i = i1;
        }
    }
}