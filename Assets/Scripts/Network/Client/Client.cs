using System;
using System.Threading;
using Events.Events;
using Network.Messages.Packets.Network;
using Players;
using World;

namespace Network.Client
{
    public class Client
    {
        private static readonly object _lock = new();
        private static Client _instance;

        private Thread _clientThread;
        private bool _disposed;

        private Client()
        {
        }

        public static Client Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance;
                }
            }
        }

        public async void Initialize()
        {
            ConnectionPacket.OnPlayerAddedCallback += PlayerManager.OnPlayerConnected;
            ConnectionPacket.OnPlayerRemovedCallback += PlayerManager.OnPlayerDisconnected;

            WorldsManager.Instance.LoadWorlds();
            await ConnectionPacket.TrySendPacket();

            new OnClientStart().Invoke();
        }


        public static Client CreateInstance()
        {
            if (_instance == null)
            {
                _instance = new Client();
                _instance.Initialize();
            }

            return _instance;
        }

    }
}