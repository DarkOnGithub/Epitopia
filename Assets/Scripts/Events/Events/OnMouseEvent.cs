using Events.EventHandler;
using Events.EventHandler.Holders;
using Events.EventHandler.Registers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Events.Events
{
    public class OnMouseEvent : IEvent
    {
        public static readonly ConditionalListenersHolder<KeyCode> Holder = new();
        public static ConditionalListenerRegister<OnMouseEvent, KeyCode> Registry = new(Holder);
        public bool IsCancelled { get; set; }
        public InputAction.CallbackContext Ctx { get; set; }
        public KeyCode Mouse { get; set; }
        public Vector2 Position { get; set; }
        
        public static bool Invoke(OnMouseEvent @event, KeyCode condition)
        {
            return Holder.Invoke(@event, condition);
        }

        public void Invoke(KeyCode condition)
        {
            Invoke(this, condition);
        }
    }
}