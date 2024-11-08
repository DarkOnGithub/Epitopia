using System;
using UnityEngine;
using World.Chunks;

namespace Utils
{
    public static class VectorUtils
    {
        private static int _chunkSize = Chunk.ChunkSize;

        public static int ToIndex(this Vector2Int vector)
        {
            return vector.x + vector.y * _chunkSize;
        }

        public static int ToIndex(this (int x, int y) vector)
        {
            return vector.x + vector.y * _chunkSize;
        }


        public static Vector2Int ToVector2Int(this int index)
        {
            return new Vector2Int(index % _chunkSize, index / _chunkSize);
        }

        public static Vector3Int ToVector3Int0(this int index)
        {
            return new Vector3Int(index % _chunkSize, index / _chunkSize);
        }

        public static int GetNearestIntDivisibleBy(int value, int divisor)
        {
            var q = value / divisor;
            var n1 = divisor * q;
            var n2 = value * divisor > 0 ? divisor * (q + 1) : value * (q - 1);
            if (Math.Abs(value - n1) < Math.Abs(value - n2)) return n1;
            return n2;
        }

        public static Vector2Int GetNearestVectorDivisibleBy(Vector2 value, int divisor)
        {
            return new Vector2Int(
                GetNearestIntDivisibleBy((int)value.x, divisor),
                GetNearestIntDivisibleBy((int)value.y, divisor)
            );
        }

        public static Vector3Int ToVector3Int(this Vector2Int vector)
        {
            return new Vector3Int(vector.x, vector.y);
        }
    }
}