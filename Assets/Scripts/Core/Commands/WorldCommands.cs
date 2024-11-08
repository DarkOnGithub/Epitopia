using System.Linq;
using Network.Messages.Packets.World;
using QFSW.QC;
using World.Worlds;

namespace Core.Commands
{
    public class WorldCommands
    {
        [Command]
        public static void SendChunk()
        {
            Overworld.Instance.SendChunkToServer(Overworld.Instance.Chunks.First().Value);
        }
    }
}