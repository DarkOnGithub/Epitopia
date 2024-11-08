using System.Linq;
using QFSW.QC;


namespace Core.Commands
{
    public class WorldCommands
    {
        [Command]
        public static void SendChunk()
        {
   //         Overworld.Instance.SendChunkToServer(Overworld.Instance.Chunks.First().Value);
        }
    }
}