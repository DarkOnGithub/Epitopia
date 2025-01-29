using System;
using System.Collections.Generic;
using Core;
using Storage;
using UnityEngine;
using World.Blocks;
using World.Chunks;

namespace World
{
    public class ServerWorldHandler : IDisposable
    {
        private static readonly BetterLogger _logger = new(typeof(ServerWorldHandler));
        private bool _disposed;

        public ServerWorldHandler(AbstractWorld worldIn)
        {
            WorldIn = worldIn;
            Storage = new WorldStorage(worldIn.Identifier.GetWorldName());
            Query = new WorldQuery(worldIn);
        }
        
        public AbstractWorld WorldIn { get; }
        public WorldStorage Storage { get; }
        public WorldQuery Query { get; }
        
        
        public void PlayerRequestChunks(ulong player, Vector2Int[] positions)
        {
            foreach (var chunk in LoadChunksInRange(positions))
            {
                AddPlayerToChunk(chunk, player);
                if (chunk.IsEmpty)
                    WorldManager.GenerateChunk(chunk);
                else
                    WorldManager.ChunkSenderQueue.Enqueue(chunk);
            }
        }

        public void AddPlayerToChunk(Chunk chunk, ulong player)  => chunk.Players.Add(player);
        

        public void RemovePlayerFromChunk(Chunk chunk, ulong player)
        {
            chunk.Players.Remove(player);
            if (chunk.Players.Count == 0) DestroyChunk(chunk);
        }

        private void DestroyChunk(Chunk chunk)
        {
            Query.RemoveChunk(chunk.Center);
            Storage.Put(chunk.Center, ChunkUtils.SerializeChunk(chunk, false));
            chunk.Dispose();
        }

        private bool GetChunkFromStorage(Vector2Int position, out Chunk chunk)
        {
            if (Storage.TryGet(position, out var bytes))
            {
                chunk = Query.CreateChunk(position, ChunkUtils.DeserializeChunk(bytes, false).BlockStates);
                return true;
            }

            chunk = null;
            return false;
        }

        public bool GetChunkFromMemoryOrStorage(Vector2Int position, out Chunk chunk)
        {
            return Query.TryGetChunk(position, out chunk) || GetChunkFromStorage(position, out chunk);
        }

        public IEnumerable<Chunk> LoadChunksInRange(Vector2Int[] positions)
        {
            foreach (var position in positions)
                if (GetChunkFromMemoryOrStorage(position, out var chunk))
                    yield return chunk;
                else
                    yield return Query.CreateEmptyChunk(position);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                Storage.Close();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~ServerWorldHandler()
        {
            Dispose(false);
        }
    }
}