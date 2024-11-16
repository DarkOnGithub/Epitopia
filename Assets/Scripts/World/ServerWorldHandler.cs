using Network.Messages.Packets.World;
using World.Chunks;

namespace World
{
    public class ServerWorldHandler : IWorldHandler
    {
        public AbstractWorld WorldIn { get; }
        
        public ServerWorldHandler(AbstractWorld worldIn)
        {
            WorldIn = worldIn;
        }
        public void OnPacketReceived(ChunkTransferMessage message)
        {
            
        }

        public void SendChunk(Chunk chunk)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveChunk(Chunk chunk)
        {
            throw new System.NotImplementedException();
        }
    }
}