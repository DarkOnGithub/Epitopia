using Event.Core;

namespace Event
{
    public class SampleEvent : Core.Event, ICancellable
    {
        public string s;
        public int i;
        public SampleEvent(string s1, int i1)
        {
            s = s1;
            i = i1;
        }
    }
}