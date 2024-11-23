using System;
using System.Threading;
using JetBrains.Annotations;
using Network.Messages;
using Network.Messages.Packets.Network;
using Unity.Netcode;
using Unity.Services.Authentication;
using World;

namespace Network.Client
{
    public class Client : IDisposable
    {
        private static readonly object _lock = new object();
        private static Client _instance;

        private Thread _clientThread;
        private bool _disposed;

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

        private Client()
        {
            WorldManager.LoadWorlds();
            Scanner.StartScheduler();
        }

        public static Client CreateInstance()
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new Client();
                }
                return _instance;
            }
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
                if (_clientThread != null && _clientThread.IsAlive)
                {
                    _clientThread.Abort();
                    _clientThread = null;
                }
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
