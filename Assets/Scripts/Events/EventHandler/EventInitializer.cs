using Tests;
using UnityEngine;

namespace Events.EventHandler
{
    public class EventInitializer : MonoBehaviour
    {
        private void Awake()
        {
            EventFactory.RegisterEvents();
            RegisterListeners();
            EventFactory.Invoke(new SampleEvent("Hello", 727));
        }

        private static void RegisterListeners()
        {
            EventFactory.Register(typeof(EventSampleStaticTest));
        }
    }
}