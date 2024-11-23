using UnityEngine;
using World;

namespace Events.EventHandler
{
    public class EventInitializer : MonoBehaviour
    {
        private void Awake()
        {
            EventFactory.RegisterEvents();
            RegisterListeners();
        }

        private static void RegisterListeners()
        {
            
        }
    }
}