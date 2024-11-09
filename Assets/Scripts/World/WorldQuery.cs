using UnityEngine;
using Utils;
using World.Blocks;
using World.Chunks;

namespace World
{

    public static class WorldQuery
    {
        private static int _chunkSize = Chunk.ChunkSize;
        public static Vector2Int FindNearestChunkPosition(Vector2 worldPosition)
        {
            return VectorUtils.GetNearestVectorDivisibleBy(worldPosition, _chunkSize);
        }
        public static bool FindNearestChunk(AbstractWorld worldIn, Vector2 worldPosition, out Chunk chunk)
        {
            var chunkPosition = VectorUtils.GetNearestVectorDivisibleBy(worldPosition, _chunkSize);
            return worldIn.Chunks.TryGetValue(chunkPosition, out chunk);
        }

        public static Vector2Int WorldToLocalPosition(Vector2 worldPosition, Vector2Int chunkPosition)
        {
            return Vector2Int.FloorToInt(worldPosition) - chunkPosition;
        }

        public static bool SetBlock(AbstractWorld worldIn, Vector2 worldPosition, IBlockState block)
        {
            if (!FindNearestChunk(worldIn, worldPosition, out var chunk)) return false;
            var localIndex = WorldToLocalPosition(worldPosition, chunk.Origin).ToIndex();
            chunk.SetBlock(localIndex, block);
            return true;
        }

        public static bool GetBlock(AbstractWorld worldIn, Vector2 worldPosition, out IBlockState block)
        {
            if (!FindNearestChunk(worldIn, worldPosition, out var chunk))
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