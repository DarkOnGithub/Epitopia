using Events;
using Events.EventHandler;
using UnityEngine;

namespace Tests
{
    public static class EventSampleStaticTest
    {
        [SubscribeEvent(Priority = EventPriority.High)]
        public static void TestHigh(SampleEvent evt)
        {
            Debug.Log(evt.i);
        }
        
        [SubscribeEvent(Priority = EventPriority.Low)]
        public static void TestLow(SampleEvent evt)
        {
            Debug.Log(evt.s);
        }
    }
}