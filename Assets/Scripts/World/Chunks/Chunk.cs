using Blocks;
using Renderer;
using UnityEngine;
using Utils.LZ4;

namespace World.Chunks
{
    public class Chunk : AbstractChunk
    {
        public World World;
        public new IBlockState[] Blocks = new IBlockState[ChunkSize * ChunkSize];
        
        public Chunk(World worldIn, Vector2Int position) : base(position)
        {
            World = worldIn;   
        }
        public Chunk(World worldIn, Vector2Int position, byte[] content) : base(position)
        {
            World = worldIn;
            var blockData = LZ4.Decompress(content);
            
        }
        public override IBlockState GetBlock(int localIndex) => Blocks[localIndex];
        public T GetBlock<T>(int localIndex) where T: IBlockState=> (T)Blocks[localIndex];

        public override void SetBlock(int localIndex, IBlockState block) => Blocks[localIndex] = block;

        public override void RemoveBlock(int localIndex) => 
            Blocks[localIndex] = BlocksRegistry.BLOCK_AIR.CreateBlockData();

        public override void DestroyChunk()
        {
            Blocks = null;
        }
        public override void Draw()
        {
            ChunkRenderer.RenderChunk(this);
        }
        public override string ToString()
        {
            return $"Chunk: {Center}";
        }
    }
}