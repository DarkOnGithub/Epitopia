using World.Blocks;
using World.Chunks;

namespace World.WorldGeneration
{
    public class WorldGeneration
    {
        public static void GenerateChunk(AbstractWorld worldIn, Chunk chunk)
        {
            chunk.SetBlock(0, BlockRegistry.BLOCK_DIRT.DefaultState);
            chunk.IsEmpty = false;
            worldIn.SendChunkToServer(chunk);
        }
    }
}