using UnityEngine;
using Utils;
using World.Blocks;
using World.Chunks;

namespace World
{

    public static class WorldUtils
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
    }
}