using System.Threading.Tasks;
using Network;
using Network.Client;
using Network.Server;
using QFSW.QC;
using UnityEngine;

namespace Core.Commands
{
    public class ServerCommand
    {
        [Command]
        public static async Task CreateServer(string name)
        {
            await LobbyCommands.CreateLobby(name, 2);
            Server.CreateInstance(name, null);
        }

        [Command]
        public static async Task JoinServer(string name)
        {
            await LobbyCommands.JoinLobbyFromName(name);
            Client.CreateInstance();
        }
    }
}