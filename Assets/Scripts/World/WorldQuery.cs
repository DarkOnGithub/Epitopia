using System.Collections.Generic;
using UnityEngine;
using World.Chunks;

namespace World
{
    public class WorldQuery
    {
        private readonly AbstractWorld _worldIn;
        public readonly Dictionary<Vector2Int, Chunk> Chunks = new();
        public WorldQuery(AbstractWorld worldIn)
        {
            _worldIn = worldIn;
        }
        
        public void RemoveChunk(Vector2Int chunkPosition) => Chunks.Remove(chunkPosition);
        public void AddChunk(Chunk chunk) => Chunks.Add(chunk.Center, chunk);
        public void TryAddChunk(Chunk chunk)
        {
            if (!Chunks.TryGetValue(chunk.Center, out var _))
                Chunks.Add(chunk.Center, chunk);
        }

        public Chunk GetChunk(Vector2Int chunkPosition) => Chunks[chunkPosition];
        public bool TryGetChunk(Vector2Int chunkPosition, out Chunk chunk) => Chunks.TryGetValue(chunkPosition, out chunk);
        public Chunk GetChunkOrCreate(Vector2Int chunkPosition)
        {
            if (!Chunks.TryGetValue(chunkPosition, out var chunk))
            {
                chunk = new Chunk(_worldIn, chunkPosition);
                Chunks.Add(chunkPosition, chunk);
            }
            return chunk;
        }
        public bool FindNearestChunk(Vector2 worldPosition, out Chunk chunk)
        {
            var chunkPosition = WorldUtils.FindNearestChunkPosition(worldPosition);
            return Chunks.TryGetValue(chunkPosition, out chunk);
        }
    }
}