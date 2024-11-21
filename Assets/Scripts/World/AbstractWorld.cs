using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using JetBrains.Annotations;
using MessagePack;
using Network.Messages;
using Network.Messages.Packets.World;
using Players;
using Unity.Netcode;
using UnityEngine;
using World.Blocks;
using World.Chunks;

namespace World
{
  
    public abstract partial class AbstractWorld
    {
        protected BetterLogger Logger = new BetterLogger(typeof(AbstractWorld));
        public readonly WorldIdentifier Identifier;
        private readonly ServerWorldHandler _serverHandler;
        private ClientWorldHandler _clientHandler;
        public ClientWorldHandler ClientHandler { get; private set; }

        public ServerWorldHandler ServerHandler
        {
            get
            {
                if(!NetworkManager.Singleton.IsHost)
                    Logger.LogWarning("ServerHandler is only available on the server");
                return _serverHandler;
            }
        }

        public WorldQuery Query;
  
        public AbstractWorld(WorldIdentifier identifier)
        {
            Identifier = identifier;
            Query = new WorldQuery(this);
            _clientHandler = new ClientWorldHandler(this);
            if(NetworkManager.Singleton.IsHost)
                _serverHandler = new ServerWorldHandler(this);
        }

        public void OnChunkUpdated(Chunk chunk)
        {
            
        }
    }
}