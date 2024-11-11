using Network.Messages.Packets.World;
using World.Chunks;

namespace World
{
    public interface IWorldHandler
    {
        AbstractWorld WorldIn { get; }
        void OnPacketReceived(ChunkTransferMessage message);
        void SendChunk(Chunk chunk);
        void RemoveChunk(Chunk chunk);
    }
}