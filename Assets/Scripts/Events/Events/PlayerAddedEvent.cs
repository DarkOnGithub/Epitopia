using Events.EventHandler;
using Players;

namespace Events.Events
{
    public class PlayerAddedEvent : Event
    {
        public Player Player;
        
        public PlayerAddedEvent(Player player)
        {
            Player = player;
        }
    }
}