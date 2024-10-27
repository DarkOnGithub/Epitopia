using Event;
using Event.Core;
using UnityEngine;

namespace Tests
{
    public class EventSampleStaticTest
    {
        [SubscribeEvent]
        public static void Test(SampleEvent evt)
        {
            Debug.Log(evt.i);
        }
    }
}