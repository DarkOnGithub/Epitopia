using Blocks;
using UnityEngine;

namespace World.Chunks
{
    public class Chunk : AbstractChunk
    {

        public new IBlockState[] Blocks = new IBlockState[ChunkSize * ChunkSize];
        
        public Chunk(Vector2Int position) : base(position)
        {
            GameObject.CreatePrimitive(PrimitiveType.Cube).transform.localScale = new Vector3(ChunkSize, ChunkSize, 1);
        }

        public override IBlockState GetBlock(int localIndex) => Blocks[localIndex];
        public T GetBlock<T>(int localIndex) where T: IBlockState=> (T)Blocks[localIndex];

        public override void SetBlock(int localIndex, IBlockState block) => Blocks[localIndex] = block;

        public override void RemoveBlock(int localIndex) => 
            Blocks[localIndex] = BlocksRegistry.BLOCK_AIR.CreateBlockData();

        public override string ToString()
        {
            return $"Chunk: {Center}";
        }
    }
}