using System;
using System.Threading;
using JetBrains.Annotations;
using Network.Messages;
using Network.Messages.Packets.Network;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;
using World;

namespace Network.Server
{
    public struct ServerInfo
    {
        public string ServerId;
        public string ServerName;
        public string OwnerId;
        public long CreationDate;
        public long LastJoined;
    }

    public class Server : IDisposable
    {
        private static readonly object _lock = new object();
        private static Server _instance;

        private Thread _worldThread;
        public ServerInfo Info { get; private set; }
        private bool _disposed;

        public static Server Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance;
                }
            }
        }

        private Server(string serverName, [CanBeNull] string serverId = null)
        {
            Debug.Log("b");
            Info = ServerUtils.GetOrCreateInfo(serverName, serverId);
            Debug.Log("a");
            WorldManager.LoadWorlds();
            Scanner.StartScheduler();
            StartServerThreads();

            foreach (var client in NetworkManager.Singleton.ConnectedClients.Keys)
            {
                OnClientAdded(client);
            }

            NetworkManager.Singleton.OnClientConnectedCallback += OnClientAdded;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientRemoved;
        }

        public static Server CreateInstance(string serverName, [CanBeNull] string serverId = null)
        {
            lock (_lock)
            {
                if (_instance == null) 
                    _instance = new Server(serverName, serverId);
                return _instance;
            }
        }

        public static async void OnClientRemoved(ulong client)
        {
            MessageFactory.SendPacket(SendingMode.ClientToClient, new ConnectionMessage
            {
                State = ConnectionState.Disconnecting,
                PlayerName = await AuthenticationService.Instance.GetPlayerNameAsync(),
                PlayerId = AuthenticationService.Instance.PlayerId,
                ClientId = client
            });
        }
        public static async void OnClientAdded(ulong client)
        {
            MessageFactory.SendPacket(SendingMode.ClientToClient, new ConnectionMessage
            {
                State = ConnectionState.Connecting,
                PlayerName = await AuthenticationService.Instance.GetPlayerNameAsync(),
                PlayerId = AuthenticationService.Instance.PlayerId,
                ClientId = client
            });
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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

        private static void StartServerThreads()
        {
            new Thread(WorldManager.StartWorldTread).Start();
        }
    }
}
