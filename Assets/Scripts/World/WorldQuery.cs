using System.Collections.Generic;
using UnityEngine;
using World.Blocks;
using World.Chunks;

namespace World
{
    public class WorldQuery
    {
        private readonly AbstractWorld _worldIn;
        public readonly Dictionary<Vector2Int, Chunk> Chunks;

        public WorldQuery(AbstractWorld worldIn, Dictionary<Vector2Int, Chunk> chunks = null)
        {
            _worldIn = worldIn;
            Chunks = chunks ?? new Dictionary<Vector2Int, Chunk>();
        }

        public void RemoveChunk(Vector2Int chunkPosition)
        {
            Chunks.Remove(chunkPosition);
        }

        public void AddChunk(Chunk chunk)
        {
            Chunks.Add(chunk.Center, chunk);
        }

        public void TryAddChunk(Chunk chunk)
        {
            if (!Chunks.TryGetValue(chunk.Center, out var _))
                Chunks.Add(chunk.Center, chunk);
        }

        public IEnumerable<Chunk> LazyGetChunks(Vector2Int[] chunkPositions)
        {
            foreach (var chunkPosition in chunkPositions)
                if (Chunks.TryGetValue(chunkPosition, out var chunk))
                    yield return chunk;
        }

        public Chunk GetChunk(Vector2Int chunkPosition)
        {
            return Chunks[chunkPosition];
        }

        public bool TryGetChunk(Vector2Int chunkPosition, out Chunk chunk)
        {
            return Chunks.TryGetValue(chunkPosition, out chunk);
        }

        public Chunk GetChunkOrCreate(Vector2Int chunkPosition)
        {
            if (!Chunks.TryGetValue(chunkPosition, out var chunk))
                return CreateEmptyChunk(chunkPosition);
            return chunk;
        }

        public Chunk CreateEmptyChunk(Vector2Int chunkPosition)
        {
            var chunk = new Chunk(_worldIn, chunkPosition);
            Chunks.Add(chunkPosition, chunk);
            return chunk;
        }

        public Chunk CreateChunk(Vector2Int chunkPosition, IBlockState[] data)
        {
            var chunk = new Chunk(_worldIn, chunkPosition, data);
            Chunks.Add(chunkPosition, chunk);
            return chunk;
        }

        public bool FindNearestChunk(Vector2 worldPosition, out Chunk chunk)
        {
            var chunkPosition = WorldUtils.FindNearestChunkPosition(worldPosition);
            return Chunks.TryGetValue(chunkPosition, out chunk);
        }
    }
}