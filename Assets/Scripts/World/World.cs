using System.Collections.Generic;
using System.Linq;
using Blocks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.XR;
using Utils;
using World.Chunks;

namespace World
{
    public abstract partial class World
    {
        public readonly Dictionary<Vector2Int, Chunk> Chunks = new();
        
        public World()
        {

        }

        [CanBeNull]
        public Chunk GetChunk(Vector2Int position)
        {
            return Chunks.GetValueOrDefault(position, null);
        }

        public IEnumerable<Chunk> GetChunks(Vector2Int[] positions)
        {
            return positions.Select(GetChunk).Where(chunk => chunk != null);
        }
        public Chunk GenerateChunk(Vector2Int position)
        {
            if (Chunks.TryGetValue(position, out var _chunk))
                return _chunk;
            var chunk = new Chunk(this, position);
            Chunks.TryAdd(position, chunk);
            return chunk;
        }

        public Chunk AddChunk(Vector2Int position, byte[] content)
        {
            if (Chunks.TryGetValue(position, out var _chunk))
                return _chunk;
            var chunk = new Chunk(this, position, content);
            Chunks.TryAdd(position, chunk);
            return chunk;
        }
        public bool SetBlock(Vector2Int worldPosition, IBlockState block)
        {
            if (WorldQuery.SetBlock(this, worldPosition, block))
            {
               return true;       
            }

            return false;
        }
    }
}