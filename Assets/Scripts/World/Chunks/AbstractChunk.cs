using Blocks;
using Mono.CSharp;
using UnityEngine;
using Utils;

namespace World.Chunks
{
    public abstract class AbstractChunk
    {
        public static readonly int ChunkSize = 32;
        public Vector2Int Center;
        public Vector2Int Origin;
        public IBlockState[] Blocks;
        public AbstractChunk(Vector2Int center)
        {
            Center = center;
            Origin = new Vector2Int(center.x - ChunkSize / 2, center.y - ChunkSize / 2);
        }

        public abstract IBlockState GetBlock(int localIndex);

        public abstract void SetBlock(int localIndex, IBlockState block);
        public abstract void RemoveBlock(int localIndex);

    }
}