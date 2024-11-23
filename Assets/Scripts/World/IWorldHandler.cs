using System;
using Network.Messages;
using Network.Messages.Packets.World;
using World.Chunks;

namespace World
{
    public interface IWorldHandler : IDisposable
    {
        AbstractWorld WorldIn { get; }
        void OnPacketReceived(NetworkUtils.Header header, ChunkTransferMessage message);
        void SendChunk(Chunk chunk, ulong[] client);
        void RemoveChunk(Chunk chunk);
    }
}