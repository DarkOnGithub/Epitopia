using Network.Messages;
using Network.Messages.Packets.World;
using UnityEngine;
using World.Chunks;

namespace World
{
    public class ClientWorldHandler : IWorldHandler
    {
        public AbstractWorld WorldIn { get; }
        
        public ClientWorldHandler(AbstractWorld worldIn)
        {
            WorldIn = worldIn;
        }
        
        public void OnPacketReceived(ChunkTransferMessage message)
        {
            var chunkData = ChunkUtils.DeserializeChunk(message.ChunkData);
            chunkData.Center = message.Center;
            var chunk = WorldIn.Query.GetChunkOrCreate(chunkData.Center);   
            var content = chunkData.BlockStates;
            if(message.IsEmpty)
                content = WorldGeneration.WorldGeneration.GenerateChunk(, WorldIn);
            chunk.UpdateContent();
        }

        public void SendChunk(Chunk chunk) => MessageFactory.SendPacket(SendingMode.ClientToServer,
            new ChunkTransferMessage
            {
                ChunkData = ChunkUtils.SerializeChunk(chunk),
                Center = chunk.Center,
                IsEmpty = chunk.IsEmpty,
                Source = PacketSouce.Client 
            }
        );

        public void RemoveChunk(Chunk chunk)
        {
            WorldIn.Query.RemoveChunk(chunk.Center);
        }
    }
}