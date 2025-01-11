using Events.EventHandler;
using Events.EventHandler.Holders;
using Events.EventHandler.Registers;

namespace Events.Events
{
    public class OnClientStart : IEvent
    {
        public static readonly DefaultListenerHolder Holder = new();
        public static DefaultListenerRegister<OnClientStart> Registry = new(Holder);
        public bool IsCancelled { get; set; }

        public static bool Invoke(OnClientStart @event)
        {
            return Holder.Invoke(@event);
        }

        public void Invoke()
        {
            Invoke(this);
        }
    }
}