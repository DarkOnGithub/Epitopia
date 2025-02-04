// using System;
// using System.Collections.Concurrent;
// using System.Collections.Generic;
// using Core.Lightning;
// using Network.Messages;
// using Network.Messages.Packets.World;
// using Players;
// using UnityEngine;
// using World.Blocks;
// using World.Chunks;
//
// namespace World
// {
//     public class ClientWorldHandler : IDisposable
//     {
//         private bool _disposed;
//         public Dictionary<int, int> SurfaceLevels = new();
//         public InputHandler InputHandler;
//         public ClientWorldHandler(AbstractWorld worldIn)
//         {
//             InputHandler = new InputHandler(this);
//             WorldIn = worldIn;
//         }
//
//         public AbstractWorld WorldIn { get; }
//
//         public void Dispose()
//         {
//             Dispose(true);
//             GC.SuppressFinalize(this);
//         }
//
//         public void OnPacketReceived(NetworkUtils.Header header, ChunkTransferMessage message)
//         {
//             var chunkData = ChunkUtils.DeserializeChunk(message.ChunkData);
//             chunkData.Center = message.Position;
//             var chunk = WorldIn.Query.GetChunkOrCreate(chunkData.Center);
//             var content = chunkData.BlockStates;
//             chunk.UpdateContent(content);
//             // LightingManager.AddLightmap(chunk);
//             //
//             // LightingManager.UpdateChunkLightmap(chunk);
//          }
//
//         public void DropChunks(Vector2Int[] positions)
//         {
//             MessageFactory.SendPacket(SendingMode.ClientToServer, new ChunkRequestMessage
//                                                                   {
//                                                                       Positions = positions,
//                                                                       Type = ChunkRequestType.Drop,
//                                                                       World = WorldIn.Identifier
//                                                                   });
//         }
//
//         public void RemoveChunk(Chunk chunk)
//         {
//             WorldIn.Query.RemoveChunk(chunk.Center);
//             DropChunks(new[] { chunk.Center });
//         }
//
//         protected virtual void Dispose(bool disposing)
//         {
//             if (_disposed)
//                 return;
//
//             if (disposing)
//                 foreach (var chunk in WorldIn.Query.Chunks.Values)
//                     chunk.Dispose();
//             _disposed = true;
//         }
//
//         ~ClientWorldHandler()
//         {
//             Dispose(false);
//         }
//     }
// }

using UnityEngine;
using World.Blocks;
using World.Chunks;

namespace World
{
    public class ClientWorldHandler
    {
        public readonly AbstractWorld WorldIn;
        public readonly WorldQuery Query;
        public ClientWorldHandler(AbstractWorld worldIn)
        {
            Query = new WorldQuery(worldIn);
            WorldIn = worldIn;
        }

        public void OnChunkReceived(Vector2Int chunkPosition, byte[] blockStates)
        {
            var chunk = Chunk.Deserialize(WorldIn, chunkPosition, blockStates);
            Query.AddChunk(chunk);
        }
        
        
    }
}