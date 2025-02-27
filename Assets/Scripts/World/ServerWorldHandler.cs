﻿// using System;
// using System.Collections.Concurrent;
// using System.Collections.Generic;
// using Core;
// using Core.Lightning;
// using Storage;
// using UnityEngine;
// using World.Blocks;
// using World.Chunks;
//
// namespace World
// {
//     public class ServerWorldHandler : IDisposable
//     {
//         private static readonly BetterLogger _logger = new(typeof(ServerWorldHandler));
//         private bool _disposed;
//         public LightingManager LightingManager;
//         public ConcurrentDictionary<Vector2Int, IBlockState> BlocksToLoad = new();
//
//         
//         
//         public ServerWorldHandler(AbstractWorld worldIn)
//         {
//             LightingManager = new LightingManager(worldIn);
//             WorldIn = worldIn;
//             Storage = new WorldStorage(worldIn.Identifier.GetWorldName());
//             Query = new WorldQuery(worldIn);
//         }
//         
//         public AbstractWorld WorldIn { get; }
//         public WorldStorage Storage { get; }
//         public WorldQuery Query { get; }
//         
//         
//         public void PlayerRequestChunks(ulong player, Vector2Int[] positions)
//         {
//             foreach (var chunk in LoadChunksInRange(positions))
//             {
//                 AddPlayerToChunk(chunk, player);
//                 if (chunk.IsEmpty)
//                     WorldManager.GenerateChunk(chunk);
//                 else
//                     WorldManager.ChunkSenderQueue.Enqueue(chunk);
//             }
//         }
//
//         public void AddPlayerToChunk(Chunk chunk, ulong player)  => chunk.Players.Add(player);
//         
//
//         public void RemovePlayerFromChunk(Chunk chunk, ulong player)
//         {
//             chunk.Players.Remove(player);
//             if (chunk.Players.Count == 0) DestroyChunk(chunk);
//         }
//
//         private void DestroyChunk(Chunk chunk)
//         {
//             Query.RemoveChunk(chunk.Center);
//             Storage.Put(chunk.Center, ChunkUtils.SerializeChunk(chunk, false));
//             chunk.Dispose();
//         }
//
//         private bool GetChunkFromStorage(Vector2Int position, out Chunk chunk)
//         {
//             if (Storage.TryGet(position, out var bytes))
//             {
//                 chunk = Query.CreateChunk(position, ChunkUtils.DeserializeChunk(bytes, false).BlockStates);
//                 return true;
//             }
//
//             chunk = null;
//             return false;
//         }
//
//         public bool GetChunkFromMemoryOrStorage(Vector2Int position, out Chunk chunk)
//         {
//             return Query.TryGetChunk(position, out chunk) || GetChunkFromStorage(position, out chunk);
//         }
//
//         public IEnumerable<Chunk> LoadChunksInRange(Vector2Int[] positions)
//         {
//             foreach (var position in positions)
//                 if (GetChunkFromMemoryOrStorage(position, out var chunk))
//                     yield return chunk;
//                 else
//                     yield return Query.CreateEmptyChunk(position);
//         }
//
//         protected virtual void Dispose(bool disposing)
//         {
//             if (_disposed) return;
//
//             if (disposing)
//             {
//                 Storage.Close();
//             }
//
//             _disposed = true;
//         }
//
//         public void Dispose()
//         {
//             Dispose(true);
//             GC.SuppressFinalize(this);
//         }
//         ~ServerWorldHandler()
//         {
//             Dispose(false);
//         }
//     }
// }

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Network.Messages;
using Network.Messages.Packets.World;
using Storage;
using Unity.Netcode;
using UnityEngine;
using Utils;
using World.Blocks;
using World.Chunks;
using World.WorldGeneration;

namespace World
{
    public class ServerWorldHandler
    {
        public readonly AbstractWorld WorldIn;
        public readonly WorldQuery Query;
        public readonly WorldStorage Storage;
        public WorldGenerator WorldGenerator;
        //public readonly LightingManager LightingManager;
        public event EventHandler OnTick;
        
        //public readonly ConcurrentDictionary<int, ConcurrentDictionary<int, byte>> HeightsPerColumn = new();
        
        //Store the chunks that are currently loaded at any step including during generation
        public readonly ConcurrentDictionary<Vector2Int, List<(int, IBlockState, bool)>> BlocksToPlace = new();        
        public ServerWorldHandler(AbstractWorld worldIn)
        {
          //  LightingManager = new LightingManager(worldIn);
            WorldGenerator = new(worldIn);
            Storage = new WorldStorage(worldIn.Identifier.GetWorldName());
            Query = new WorldQuery(worldIn);
            WorldIn = worldIn;
            WorldsManager.Instance.StartCoroutine(ClockScheduler());
        }
            
        public void PlayerRequestChunks(ulong playerId, Vector2Int[] positions)
        {
            var worldsManager = WorldsManager.Instance;
            var index = 0;
            
            foreach (var chunk in Query.GetChunks(positions))
            {
                var position = positions[index++];
                Chunk newChunk = chunk;
                if (newChunk is null && !TryGetChunkFromStorage(position, out newChunk))
                {
                    worldsManager.EnqueueChunkGeneration(WorldIn, position, playerId);
                    continue;
                }  
                
                newChunk.AddPlayer(playerId);
                worldsManager.EnqueueChunkToDispatch(chunk);
            }
        }

        public void PlayerDropChunks(ulong playerId, Vector2Int[] positions)
        {
            foreach (var chunk in Query.GetChunks(positions)) 
                chunk?.RemovePlayer(playerId);
        }
        
        
        private bool TryGetChunkFromStorage(Vector2Int position, out Chunk chunk)
        {
            if (Storage.TryGet(position, out var bytes))
            {
                chunk = Chunk.Deserialize(WorldIn, position, bytes);
                return true;
            }
            chunk = null;
            return false;
        }

        public void PlaceBlockFromWorldPosition(Vector2Int worldPosition, IBlockState blockState, bool replace = false)
        {
            var chunkPosition = VectorUtils.GetNearestChunkPosition(worldPosition);
            var localPosition = VectorUtils.WorldPositionToLocalPosition(worldPosition, chunkPosition);
            if (Query.TryGetChunk(chunkPosition, out var chunk))
                chunk.TryPlaceBlockAt(blockState, localPosition.ToIndex(), false, replace);
            else
            {
                BlocksToPlace.AddOrUpdate(
                    chunkPosition,
                    _ => new List<(int, IBlockState, bool)> { (localPosition.ToIndex(), blockState, replace) },
                    (_, existingList) =>
                    {
                        lock (existingList) 
                            existingList.Add((localPosition.ToIndex(), blockState, replace));
                        return existingList;
                    }
                );
            }
            
        }

        public IEnumerator ClockScheduler()
        {
            while (true)
            {
                yield return new WaitForSeconds(1 / 30f);
                OnTick?.Invoke(this, EventArgs.Empty);
            }
        } 
        
        
        //!TODO later
        public void Destroy()
        {
            Query.Destroy();
        }
        
    }
}