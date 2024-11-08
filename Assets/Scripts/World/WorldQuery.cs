using System.Collections.Generic;
using Blocks;
using UnityEngine;
using Utils;
using World.Chunks;

namespace World
{
    public static class WorldQuery
    {
        private static int _chunkSize = AbstractChunk.ChunkSize;
        public static bool FindNearestChunk(World world, Vector2 worldPosition, out Chunk chunk)
        {
            var chunkPosition = VectorUtils.GetNearestVectorDivisibleBy(worldPosition, _chunkSize);
            return world.Chunks.TryGetValue(chunkPosition, out chunk);
        }
        
        public static Vector2Int WorldToLocalPosition(Vector2 worldPosition, Vector2Int chunkPosition)
        {
            return Vector2Int.FloorToInt(worldPosition) - chunkPosition;
        }
        public static bool SetBlock(World world, Vector2 worldPosition, IBlockState block)
        {
            if (!FindNearestChunk(world, worldPosition, out var chunk)) return false;
            var localIndex = WorldToLocalPosition(worldPosition, chunk.Origin).ToIndex();
            chunk.SetBlock(localIndex, block);
            return true;
        }
        
        public static bool GetBlock(World world, Vector2 worldPosition, out IBlockState block)
        {
            if (!FindNearestChunk(world, worldPosition, out var chunk))
            {
                block = null;
                return false;
            }

            var localIndex = WorldToLocalPosition(worldPosition, chunk.Origin).ToIndex();
            block = chunk.GetBlock(localIndex);
            return true;
        }
    }
}