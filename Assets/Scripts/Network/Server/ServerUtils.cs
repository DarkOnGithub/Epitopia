using System;
using JetBrains.Annotations;
using Network.Lobby.Authentification;
using Network.Server;
using Unity.Netcode;
using Unity.Services.Authentication;

namespace Network
{
    public static class ServerUtils
    {
        public static ServerInfo GetOrCreateInfo(string serverName, [CanBeNull] string serverId = null)
        {
            if (serverId != null && ServerManager.Servers.ContainsKey(serverId)) throw new NotImplementedException();

            return new ServerInfo()
            {
                ServerId = new Guid().ToString(),
                ServerName = serverName,
                OwnerId = Authentification.LocalPlayerId,
                CreationDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                LastJoined = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };
        }
    }
}