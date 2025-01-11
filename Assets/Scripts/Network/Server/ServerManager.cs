using System.Collections.Generic;
using Core;
using Unity.Netcode;

namespace Network
{
    public class ServerManager : NetworkBehaviour
    {
        public static readonly string ServersFilePath = FilesManager.DataPath + "/Servers.json";
        public static Dictionary<string, string> Servers;

        private void Awake()
        {
            ServerFiles.LoadServers();
        }
    }
}