using Events.EventHandler;
using Events.EventHandler.Holders;
using Events.EventHandler.Registers;
using UnityEngine.InputSystem;

namespace Events.Events
{
    public class OnKeyEvent : IEvent
    {
        public static readonly ConditionalListenersHolder<Key> Holder = new();
        public static ConditionalListenerRegister<OnKeyEvent, Key> Registry = new(Holder);
        public bool IsCancelled { get; set; }
        public InputAction.CallbackContext Ctx { get; set; }
        public Key Key { get; set; }
        
        public static bool Invoke(OnKeyEvent @event, Key condition)
        {
            return Holder.Invoke(@event, condition);
        }

        public void Invoke(Key condition)
        {
            Invoke(this, condition);
        }
    }
}