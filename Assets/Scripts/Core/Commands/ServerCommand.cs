using System.Threading.Tasks;
using Network.Client;
using Network.Server;
using QFSW.QC;

namespace Core.Commands
{
    public class ServerCommand
    {
        [Command]
        public static async Task CreateServer(string name)
        {
            await LobbyCommands.CreateLobby(name, 4);
            Server.CreateInstance(name);
        }

        [Command]
        public static async Task JoinServer(string name)
        {
            await LobbyCommands.JoinLobbyFromName(name);
            Client.CreateInstance();
        }
    }
}