using UnityEngine;
using World;

namespace Old
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