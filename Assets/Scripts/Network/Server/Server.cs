using System;
using System.IO;
using System.Threading;
using Events.Events;
using JetBrains.Annotations;
using Network.Messages.Packets.Network;
using Players;
using UnityEngine;
using Utils;
using World;
using World.WorldGeneration;

namespace Network.Server
{
    public class Server : IDisposable
    {
        private static readonly object _lock = new();
        private static Server _instance;
        private bool _disposed;

        private Thread _worldThread;

        private Server(string serverName, [CanBeNull] string serverId = null)
        {
            Info = ServerUtils.GetOrCreateInfo(serverName, serverId);
        }

        public ServerInfo Info { get; }
        public string ServerDirectory => $"{Application.persistentDataPath}/{Info.ServerName}";
        public string ConfigDirectory => $"{ServerDirectory}/Config";

        public static Server Instance
        {
            get
            {
                if (_instance == null)
                    throw new NullReferenceException("Server instance is null");
                return _instance;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void InitializeConfig()
        {
            FileUtils.CloneDirectory(Application.streamingAssetsPath + "/Config",
                                     ServerDirectory + "/Config");
        }

        public async void Initialize()
        {
            Seed.Initialize(Info.ServerName.GetHashCode());
            InitializeConfig();

            ConnectionPacket.OnPlayerAddedCallback += PlayerManager.OnPlayerConnected;
            ConnectionPacket.OnPlayerRemovedCallback += PlayerManager.OnPlayerDisconnected;
            WorldManager.LoadWorlds();

            StartServerThreads();
            await ConnectionPacket.TrySendPacket();
            new OnClientStart().Invoke();
        }

        public static Server CreateInstance(string serverName, [CanBeNull] string serverId = null)
        {
            if (_instance == null)
            {
                _instance = new Server(serverName, serverId);
                _instance.Initialize();
            }

            return _instance;
        }

        private static void StartServerThreads()
        {
            new Thread(WorldManager.ChunksDispatcher).Start();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                if (_worldThread != null && _worldThread.IsAlive)
                {
                    _worldThread.Abort();
                    _worldThread = null;
                }

                WorldManager.UnloadWorlds();
            }

            _disposed = true;
            lock (_lock)
            {
                _instance = null;
            }
        }

        ~Server()
        {
            Dispose(false);
        }
    }

    public struct ServerInfo
    {
        public string ServerId;
        public string ServerName;
        public string OwnerId;
        public long CreationDate;
        public long LastJoined;
    }
}