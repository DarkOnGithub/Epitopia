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
    public class Server
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

        public void InitializeConfig()
        {
            FileUtils.CloneDirectory(Application.streamingAssetsPath + "/Config",
                                     ServerDirectory + "/Config");
        }

        public async void Initialize()
        {
            Seed.Initialize(Info.ServerName.GetHashCode());
            InitializeConfig();
            WorldsManager.Instance.Init();
            ConnectionPacket.OnPlayerAddedCallback += PlayerManager.OnPlayerConnected;
            ConnectionPacket.OnPlayerRemovedCallback += PlayerManager.OnPlayerDisconnected;
            WorldsManager.Instance.LoadWorlds();

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