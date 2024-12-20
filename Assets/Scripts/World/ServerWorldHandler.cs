using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using Network.Messages;
using Network.Messages.Packets.World;
using PimDeWitte.UnityMainThreadDispatcher;
using Storage;
using Unity.VisualScripting;
using UnityEngine;
using World.Chunks;

namespace World
{
    public class ServerWorldHandler : IDisposable
    {
        private static readonly BetterLogger _logger = new(typeof(ServerWorldHandler));
        public AbstractWorld WorldIn { get; }
        private Dictionary<Vector2Int, Chunk> _chunks = new();
        public WorldStorage Storage { get; }
        public WorldQuery Query { get; }
        private bool _disposed = false;

        public ServerWorldHandler(AbstractWorld worldIn)
        {
            WorldIn = worldIn;
            Storage = new WorldStorage(worldIn.Identifier.GetWorldName());
            Query = new WorldQuery(worldIn, _chunks);
        }

        public void RemoveChunk(Chunk chunk)
        {
            // Implementation needed
        }

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

        public void AddPlayerToChunk(Chunk chunk, ulong player)
        {
            chunk.Players.Add(player);
        }

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                foreach (var chunk in _chunks.Values) DestroyChunk(chunk);
                _chunks = null;
                Storage.Close();
            }

            _disposed = true;
        }

        ~ServerWorldHandler()
        {
            Dispose(false);
        }
    }
}