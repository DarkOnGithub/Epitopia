using System.Collections.Generic;
using Network.Messages;
using Network.Messages.Packets.World;
using Unity.Netcode;
using UnityEngine;
using World.Chunks;

namespace World
{
    public abstract partial class AbstractWorld
    {
        
        public void UpdateChunk(Chunk chunk)
        {
            
        }
        
        public void SendChunkToServer(Chunk chunk)
        {
            var packet = new ChunkSenderMessage
            {
                Position = chunk.Center,
                Data = ChunkUtils.SerializeAndCompressChunk(chunk),
                World = Identifier
            };
            MessageFactory.SendPacket(SendingMode.ClientToServer, packet, null, null, NetworkDelivery.ReliableFragmentedSequenced);
        }

        public void SendChunkToClients(Chunk chunk, ulong[] clients)
        {
            var packet = new ChunkSenderMessage
                         {
                             Position = chunk.Center,
                             Data = ChunkUtils.SerializeAndCompressChunk(chunk),
                             World = Identifier
                         };
            MessageFactory.SendPacket(SendingMode.ServerToClient, packet, clients, null, NetworkDelivery.ReliableFragmentedSequenced);
        }
        private Chunk ReceiveChunk(ChunkSenderMessage message)
        {
            var chunkData = ChunkUtils.DeserializeAndDecompressChunk(message.Data);
            return CreateChunk(chunkData.Center, chunkData); 
        }
        public void ReceiveChunkFromClient(ChunkSenderMessage message)
        {
            var chunk = ReceiveChunk(message);
            if(chunk != null)
                HostChunks.TryAdd(message.Position, chunk);
        }
        public void ReceiveChunkFromServer(ChunkSenderMessage message)
        {
            ReceiveChunk(message);
        }
        public Chunk GetChunkOrCreateIt(Vector2Int center)
        {
            if (HostChunks.TryGetValue(center, out var chunk)) //!TODO Search on db
                return chunk;
            return CreateChunk(center);
        }
        public void RequestChunks(Vector2Int[] positions, ulong[] clients)
        {
            foreach (var position in positions)
            {
                SendChunkToClients(GetChunkOrCreateIt(position),clients);
            }
            
        }
        
    }
}