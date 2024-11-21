using System.Threading;

namespace Network
{
    public struct ServerInfo
    {
        public string ServerId;
        public string ServerName;
        public string OwnerId;
        public long CreationDate;
        public long LastJoined;
    }

    public class Server
    {
        private static Server _instance;
        private static readonly object _lock = new object();
        private Thread _worldThread;

        private Server(string serverId)
        {
            
        }

        public static Server Instance(string serverId)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new Server(serverId);
                }
                return _instance;
            }
        }

        public static void StartServerThreads()
        {
            new Thread(StartWorldTread).Start();
        }

        public static void StartWorldTread()
        {
            while (true)
            {
                Thread.Sleep(100 / 3);
            }
        }
    }
}