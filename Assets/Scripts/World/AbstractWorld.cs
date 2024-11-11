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
    // public class WorldHostController
    // {
    //     private AbstractWorld _world;
    //     public Dictionary<Vector2Int, Chunk> Chunks = new();            
    //     public WorldHostController(AbstractWorld world)
    //     {
    //         _world = world;
    //     }
    //
    //     public void DeleteChunk(Chunk chunk)
    //     {
    //         Chunks.Remove(chunk.Center);
    //     }
    //     public void UpdateChunkOnAllClients(Chunk chunk)
    //     {
    //         SendChunkToClients(chunk, chunk.Owners.ToArray());
    //     }
    //     public void GetChunksInRange(Vector2Int[] positions, ulong client)
    //     {
    //         foreach (var position in positions)
    //         {
    //             Chunk chunk;
    //             if (!Chunks.TryGetValue(position, out chunk))
    //             {
    //                 chunk = new Chunk(_world, position);
    //                 Chunks.TryAdd(position, chunk);
    //             }
    //             chunk.CurrentGenerator = client;
    //             SendChunkToClients(chunk, new[] {client});
    //             //!TODO Search in db
    //         }
    //     }
    //     public void OnChunkReceived(byte[] chunkData, Vector2Int center, ulong client)
    //     {
    //         if (Chunks.TryGetValue(center, out var chunk))
    //         {
    //             if ((chunk.IsEmpty && chunk.CurrentGenerator == client) || !chunk.IsEmpty)
    //             {
    //                 var data = ChunkUtils.DeserializeChunk(chunkData);
    //                 chunk.BlockStates = data.BlockStates;
    //                 chunk.IsEmpty = false;
    //                 SendChunkToClients(chunk, chunk.Owners.Where(id => id != client).ToArray());
    //             }
    //         }
    //     }
    //         
    //     public void SendChunkToClients(Chunk chunk, ulong[] clients)
    //     {
    //         var packet = new ChunkTransferMessage
    //         {
    //             ChunkData = ChunkUtils.SerializeChunk(chunk),
    //             Center = chunk.Center,
    //             IsEmpty = chunk.IsEmpty,
    //             Source = PacketSouce.Server
    //         };
    //         MessageFactory.SendPacket(SendingMode.ServerToClient, packet, null, null, NetworkDelivery.ReliableFragmentedSequenced);
    //     }
    // }
    
    public abstract partial class AbstractWorld
    {
        protected BetterLogger Logger = new BetterLogger(typeof(AbstractWorld));
        public  readonly WorldIdentifier Identifier;
        public WorldQuery Query;
//        public WorldHostController HostController;
  
        public AbstractWorld(WorldIdentifier identifier)
        {
            Identifier = identifier;
            Query = new WorldQuery(this);
  //          HostController = new WorldHostController(this);
        }
        
    }
}