using System;
using UnityEngine;
using World.Blocks;
using World.Chunks;

namespace World.WorldGeneration
{
    public static class WorldGeneration
    {
        private static IBlockState[] _emptyChunk = new IBlockState[Chunk.ChunkSizeSquared];
        static WorldGeneration()
        {
            for (int i = 0; i < Chunk.ChunkSizeSquared; i++)
            {
                _emptyChunk[i] = BlockRegistry.BLOCK_AIR.GetDefaultState();
            }
        }
        
        public static IBlockState[] GenerateChunk(AbstractWorld worldIn, Vector2Int origin)
        {
            IBlockState[] chunkData = (IBlockState[])_emptyChunk.Clone();
            var random = new System.Random().Next(-2,2);
            for (int x = 0; x < Chunk.ChunkSize; x++)
            {
                for (int y = 0; y < Chunk.ChunkSize; y++)
                {
                    float noise = Mathf.PerlinNoise((x + origin.x + random) * 0.1f, (y + origin.y + random) * 0.1f);
                    int height = Mathf.FloorToInt(noise * Chunk.ChunkSize);

                    if (y + origin.y < height)
                    {
                        chunkData[x + y * Chunk.ChunkSize] = BlockRegistry.BLOCK_DIRT.GetDefaultState();
                    }
                }
            }
            chunk.IsEmpty = false;
            worldIn.SendChunkToServer(chunk);
        }
    }
}