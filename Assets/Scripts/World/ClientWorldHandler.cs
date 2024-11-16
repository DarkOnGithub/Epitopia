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
        
        public void OnPacketReceived(NetworkUtils.Header header, ChunkTransferMessage message)
        {
            var chunkData = ChunkUtils.DeserializeChunk(message.ChunkData);
            chunkData.Center = message.Center;
            var chunk = WorldIn.Query.GetChunkOrCreate(chunkData.Center);   
            var content = chunkData.BlockStates;
            if (message.IsEmpty)
                content = WorldGeneration.WorldGeneration.GenerateChunk(WorldIn, chunk.Origin); 
            chunk.UpdateContent(content);
            SendChunk(chunk);
        }

        public void SendChunk(Chunk chunk, ulong[] client = null) => MessageFactory.SendPacket(SendingMode.ClientToServer,
            new ChunkTransferMessage
            {
                ChunkData = ChunkUtils.SerializeChunk(chunk),
                Center = chunk.Center,
                IsEmpty = chunk.IsEmpty,
                Source = PacketSource.Client
            }
        );
        
        public void DropChunks(Vector2Int[] positions)
        {
            MessageFactory.SendPacket(SendingMode.ClientToServer, new ChunkRequestMessage
            {
                Positions = positions,
                Type = ChunkRequestType.Drop,
                World = WorldIn.Identifier
            });
        }
        
        public void RemoveChunk(Chunk chunk)
        {
            WorldIn.Query.RemoveChunk(chunk.Center);
            DropChunks(new []{chunk.Center});   
        }
    }
}