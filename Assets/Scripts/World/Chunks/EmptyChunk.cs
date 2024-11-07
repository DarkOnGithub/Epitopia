using System;
using Blocks;
using UnityEngine;

namespace World.Chunks
{
    public class EmptyChunk : AbstractChunk
    {
        public new IBlockState[] Blocks = Array.Empty<IBlockState>();
        
        public EmptyChunk(Vector2Int position) : base(position)
        {
            
        }

        public override IBlockState GetBlock(int localIndex) => 
            BlocksRegistry.BLOCK_AIR.CreateBlockData();
        
        public override void SetBlock(int localIndex, IBlockState block) { }

        public override void RemoveBlock(int localIndex) { }

        public override string ToString()
        {
            return $"Empty chunk: {Center}";
        }
    }
}