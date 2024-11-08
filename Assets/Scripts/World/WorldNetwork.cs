using Network.Messages;
using Network.Messages.Packets.World;
using Unity.Netcode;
using UnityEngine;
using Utils.LZ4;
using World.Chunks;

namespace World
{
    public abstract partial class World
    {

        public byte[] SerializeChunk(Chunk chunk)
        {
            return MessagePack.MessagePackSerializer.Serialize(chunk.Blocks);
        }
        public void SendChunkToServer(Chunk chunk)
        {
            var packet = new ChunkData
            {
                Position = chunk.Center,
                Data = LZ4.Compress(SerializeChunk(chunk))
            };
            MessageFactory.SendPacket(SendingMode.ClientToServer, packet);
        }
    }
}