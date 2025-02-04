using System;
using UnityEngine;
using World.Chunks;

namespace Utils
{
    public static class VectorUtils
    {
        private static readonly int _chunkSize = Chunk.ChunkSize;

        public static int ToIndex(this Vector2Int vector)
        => vector.x + vector.y * _chunkSize;
        

        public static int ToIndex(this (int x, int y) vector) => vector.x + vector.y * _chunkSize;
        

        public static Vector2Int ToVector2Int(this int index)
            => new Vector2Int(index % _chunkSize, index / _chunkSize);

        public static Vector3Int ToVector3Int0(this int index)
            => new Vector3Int(index % _chunkSize, index / _chunkSize);

        public static Vector2Int GetNearestChunkPosition(Vector2 worldPosition) => new Vector2Int(
            Mathf.FloorToInt(worldPosition.x / _chunkSize) * _chunkSize,
            Mathf.FloorToInt(worldPosition.y / _chunkSize) * _chunkSize
        );
        
        
        public static Vector3Int ToVector3Int(this Vector2Int vector)
            => new Vector3Int(vector.x, vector.y);

        public static int Serialize(this Vector2Int vector)
                => (vector.x << 16) | (vector.y & 0xFFFF);
        

        public static Vector2Int Deserialize(this int value)
            => new Vector2Int(value >> 16, value & 0xFFFF);
        
        public static Vector2Int WorldPositionToLocalPosition(Vector2Int worldPosition, Vector2Int chunkPosition)
            => worldPosition - chunkPosition;
    }
}