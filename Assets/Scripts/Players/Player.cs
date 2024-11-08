
using System.Numerics;

namespace Players
{
    public class Player
    {
        public string PlayerName;
        public string PlayerId;
        public ulong ClientId;
        public Vector2 Position;
        
        public Player(string playerName, string playerId, ulong clientId)
        {
            PlayerName = playerName;
            PlayerId = playerId;
            ClientId = clientId;
        }
        
    }
}