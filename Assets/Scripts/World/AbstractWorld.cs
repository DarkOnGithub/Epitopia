using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using Core;
using JetBrains.Annotations;
using MessagePack;
using Network;
using Network.Server;
using Network.Messages;
using Network.Messages.Packets.World;
using Players;
using Unity.Netcode;
using UnityEngine;
using World.Blocks;
using World.Chunks;

namespace World
{
    public abstract class AbstractWorld : IDisposable
    {
        protected BetterLogger Logger = new BetterLogger(typeof(AbstractWorld));
        public readonly WorldIdentifier Identifier;
        private readonly ServerWorldHandler _serverHandler;
        private ClientWorldHandler _clientHandler;

        public ClientWorldHandler ClientHandler
        {
            get
            {
                return _clientHandler;
            }
        }

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
        private bool _disposed;

        public AbstractWorld(WorldIdentifier identifier)
        {
            Debug.Log("aaaa");
            Identifier = identifier;
            Query = new WorldQuery(this);
            _clientHandler = new ClientWorldHandler(this);
            if(NetworkManager.Singleton.IsHost)
                _serverHandler = new ServerWorldHandler(this);
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _clientHandler = null;
            }

            _disposed = true;
        }

        ~AbstractWorld()
        {
            Dispose(false);
        }
    }
}