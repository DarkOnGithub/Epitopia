using System;
using System.Collections.Generic;
using System.IO;
using Core;
using Newtonsoft.Json;
using Unity.Netcode;
using UnityEngine;

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