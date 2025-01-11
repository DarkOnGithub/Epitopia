using System;
using System.Threading;
using Events.Events;
using Network.Messages.Packets.Network;
using Players;
using World;

namespace Network.Client
{
    public class Client : IDisposable
    {
        private static readonly object _lock = new();
        private static Client _instance;

        private Thread _clientThread;
        private bool _disposed;

        private Client()
        {
        }

        // Singleton property for controlled access
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public async void Initialize()
        {
            ConnectionPacket.OnPlayerAddedCallback += PlayerManager.OnPlayerConnected;
            ConnectionPacket.OnPlayerRemovedCallback += PlayerManager.OnPlayerDisconnected;

            WorldManager.LoadWorlds();
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

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
                if (_clientThread != null && _clientThread.IsAlive)
                {
                    _clientThread.Abort();
                    _clientThread = null;
                }

            _disposed = true;
            lock (_lock)
            {
                _instance = null;
            }
        }

        ~Client()
        {
            Dispose(false);
        }
    }
}