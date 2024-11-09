using System.Collections.Generic;
using System.Linq;
using Network.Messages;
using Network.Messages.Packets.World;
using Unity.Netcode;
using UnityEngine;
using World.Chunks;

namespace World
{
    public abstract partial class AbstractWorld
    {
        
        public void OnChunkUpdated(Chunk chunk)
        {
            SendChunkToClients(chunk, chunk.Owners.ToArray());
        }
        
        public void SendChunkToServer(Chunk chunk)
        {
            var packet = new ChunkSenderMessage
            {
                Position = chunk.Center,
                Data = ChunkUtils.SerializeAndCompressChunk(chunk),
                World = Identifier,
                IsEmpty = chunk.IsEmpty
            };
            MessageFactory.SendPacket(SendingMode.ClientToServer, packet, null, null, NetworkDelivery.ReliableFragmentedSequenced);
        }

        public void SendChunkToClients(Chunk chunk, ulong[] clients)
        {
            var packet = new ChunkSenderMessage
                         {
                             Position = chunk.Center,
                             Data = ChunkUtils.SerializeAndCompressChunk(chunk),
                             World = Identifier,
                             IsEmpty = chunk.IsEmpty
                         };
            MessageFactory.SendPacket(SendingMode.ServerToClient, packet, clients, null, NetworkDelivery.ReliableFragmentedSequenced);
        }
        private Chunk ReceiveChunk(ChunkSenderMessage message)
        {
            WorldManager.WaitingRoom.Remove(message.Position);
            if (message.IsEmpty)
            {
                var chunk = CreateChunk(message.Position);
                chunk.Generate(); //!TODO Generate chunk
                return chunk;
            }
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
        public Chunk GetChunkOrCreateIt(Vector2Int center, ulong[] clients)
        {
            if (HostChunks.TryGetValue(center, out var chunk)) //!TODO Search on db
            {
                chunk.AddOwners(clients);
                return chunk;
            }
            var createdChunk = CreateChunk(center);
            createdChunk.AddOwners(clients);
            return createdChunk;
        }
        public void RequestChunksHandler(Vector2Int[] positions, ulong[] clients)
        {
            foreach (var position in positions)
                SendChunkToClients(GetChunkOrCreateIt(position, clients),clients);
        }
        
        public void RequestChunks(Vector2Int[] positions, ulong[] clients)
        {
            MessageFactory.SendPacket(SendingMode.ClientToServer, new ChunkRequestMessage
            {
                RequestType = ChunkRequestType.Request,
                Positions = positions,
                World = Identifier
            });
        }
        public void DropChunks(Vector2Int[] positions, ulong[] clients)
        {
            foreach (var position in positions)
                if (Chunks.TryGetValue(position, out var chunk))
                    chunk.RemoveOwners(clients);
        }
    }
}