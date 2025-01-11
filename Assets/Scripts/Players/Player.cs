using UnityEngine;
using World;

namespace Players
{
    public class Player
    {
        public ulong ClientId;
        public string PlayerId;
        public string PlayerName;
        public Vector2 Position;
        public AbstractWorld World;


        public Player(string playerName, string playerId, ulong clientId)
        {
            PlayerName = playerName;
            PlayerId = playerId;
            ClientId = clientId;
            World = WorldManager.GetWorld(WorldIdentifier.Overworld);
            PlayerManager.Players.Add(this);
        }
    }
}