using System.Collections.Generic;
using System.Linq;
using Core;
using JetBrains.Annotations;
using MessagePack;
using Network.Messages;
using Network.Messages.Packets.World;
using Players;
using Unity.Netcode;
using UnityEngine;
using World.Blocks;
using World.Chunks;

namespace World
{
    public class WorldHostController
    {
        private AbstractWorld _world;
        public Dictionary<Vector2Int, Chunk> Chunks = new();            
        public WorldHostController(AbstractWorld world)
        {
            _world = world;
        }

        public void UpdateChunkOnAllClients(Chunk chunk)
        {
            SendChunkToClients(chunk, chunk.Owners.ToArray());
        }
        public void GetChunksInRange(Vector2Int[] positions, ulong client)
        {
            foreach (var position in positions)
            {
                if (Chunks.TryGetValue(position, out var chunk))
                    SendChunkToClients(chunk, new[] {client});
                SendChunkToClients(new Chunk(_world, position), new[] {client});
                //!TODO Search in db
            }
        }
        public void OnChunkReceived(byte[] chunkData, Vector2Int center, ulong client)
        {
            if (Chunks.TryGetValue(center, out var chunk))
            {
                if (chunk.IsEmpty && chunk.CurrentGenerator == client)
                {
                    var data = ChunkUtils.DecompressChunkContent(chunkData);
                    chunk.BlockStates = MessagePackSerializer.Deserialize<IBlockState[]>(data);
                    chunk.IsEmpty = false;
                    SendChunkToClients(chunk, chunk.Owners.Where(id => id != client).ToArray());
                }
            }
        }
            
        public void SendChunkToClients(Chunk chunk, ulong[] clients)
        {
            var packet = new ChunkTransferMessage
            {
                ChunkData = ChunkUtils.SerializeAndCompressChunk(chunk),
                Center = chunk.Center,
                IsEmpty = chunk.IsEmpty,
                Source = PacketSouce.Client
            };
            MessageFactory.SendPacket(SendingMode.ServerToClient, packet, null, null, NetworkDelivery.ReliableFragmentedSequenced);
        }
    }
    
    public abstract partial class AbstractWorld
    {
        protected BetterLogger Logger = new BetterLogger(typeof(AbstractWorld));
        
        public Dictionary<Vector2Int, Chunk> Chunks = new();
        public WorldIdentifier Identifier;
        public WorldHostController HostController;
  
        public AbstractWorld(WorldIdentifier identifier)
        {
            Identifier = identifier;
            HostController = new WorldHostController(this);
        }

        public void AddChunk(Chunk chunk)
        {
            Chunks.TryAdd(chunk.Center, chunk);
        }
        
        [CanBeNull] public Chunk GetChunk(Vector2Int position)
        {
            return Chunks.GetValueOrDefault(position, null);
        }
        
        public bool TryGetChunk(Vector2Int position, [CanBeNull] out Chunk chunk)
        {
            return Chunks.TryGetValue(position, out chunk);
        }
    }
}