
using UnityEngine;
using World;

namespace Players
{
    public class Player
    {
        public string PlayerName;
        public string PlayerId;
        public ulong ClientId;
        public Vector2 Position;
        public AbstractWorld World;
        
        
        public Player(string playerName, string playerId, ulong clientId)
        {
            PlayerName = playerName;
            PlayerId = playerId;
            ClientId = clientId;
            PlayerManager.Players.Add(this);
        }
        
    }
}