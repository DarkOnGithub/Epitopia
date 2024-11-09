using UnityEngine;
using World.Blocks;
using World.Chunks;

namespace World.WorldGeneration
{
    public class WorldGeneration
    {
        public static void GenerateChunk(AbstractWorld worldIn, Chunk chunk)
        {
            var random = new System.Random().Next(-2,2);
            for (int x = 0; x < Chunk.ChunkSize; x++)
            {
                for (int y = 0; y < Chunk.ChunkSize; y++)
                {
                    float noise = Mathf.PerlinNoise((x + chunk.Origin.x + random) * 0.1f, (y + chunk.Origin.y + random) * 0.1f);
                    int height = Mathf.FloorToInt(noise * Chunk.ChunkSize);

                    if (y + chunk.Origin.y < height)
                    {
                        chunk.SetBlock(x + y * Chunk.ChunkSize, BlockRegistry.BLOCK_DIRT.GetDefaultState());
                    }
                }
            }
            chunk.IsEmpty = false;
            worldIn.SendChunkToServer(chunk);
        }
    }
}