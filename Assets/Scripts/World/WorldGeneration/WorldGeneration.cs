using Utils;
using World.Chunks;

namespace World.WorldGeneration
{
    public static class WorldGeneration
    {
        private static int _chunkSize = AbstractChunk.ChunkSize;
        public static void GenerateChunk(Chunk chunk)
        {
            var block = Blocks.BlocksRegistry.BLOCK_DIRT.CreateBlockData();
            var random = new System.Random();
            for(int x = 0; x < _chunkSize; x++)
            {
                for(int y = 0; y < _chunkSize; y++)
                {
                    if(y + chunk.Origin.y <= 0 && random.Next(0, 2) < 1)
                        chunk.SetBlock((x, y).ToIndex(), block);
                }
            }
        }
    }
}