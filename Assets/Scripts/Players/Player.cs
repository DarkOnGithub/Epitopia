﻿
using Events.EventHandler;
using Events.Events;
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
            World = WorldManager.GetWorld(WorldIdentifier.Overworld);
            PlayerManager.Players.Add(this);
        }
        
    }
}
