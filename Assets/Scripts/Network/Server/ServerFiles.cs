using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Network
{
    public static class ServerFiles
    {
        public static void LoadServers()
        {
            if (File.Exists(ServerManager.ServersFilePath))
            {
                string json = File.ReadAllText(ServerManager.ServersFilePath);
                ServerManager.Servers = JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
            }
            else
                ServerManager.Servers = new Dictionary<string, string>();
        }

        public static  void SaveServers()
        {
            string json = JsonConvert.SerializeObject(ServerManager.Servers, Formatting.Indented);
            File.WriteAllText(ServerManager.ServersFilePath, json);
        }

        public static  void AddServer(string serverName)
        {
            string id;
            do
            {
                id = Guid.NewGuid().ToString();
            } while (ServerManager.Servers.ContainsKey(id));

            ServerManager.Servers[id] = serverName;
            SaveServers();
        }

        public static  Dictionary<string, string> GetAllServers()
        {
            return new Dictionary<string, string>(ServerManager.Servers);
        }
    }
}