using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Utils;
using World.Blocks;
using World.Chunks;
using World.WorldGeneration.Noise;
using Random = System.Random;

namespace World.WorldGeneration
{
    public static class WorldGeneration
    {
        private static IBlockState[] _emptyChunk = new IBlockState[Chunk.ChunkSizeSquared];

        static WorldGeneration()
        {
            for (var i = 0; i < Chunk.ChunkSizeSquared; i++) _emptyChunk[i] = BlockRegistry.BLOCK_AIR.GetDefaultState();
        }

        public static async Task GenerateChunk(AbstractWorld worldIn, Chunk chunk)
        {
            var chunkData = await GenerateChunk(worldIn, chunk.Origin, worldIn.WorldGenerator.HeightMapRouter);
            chunk.UpdateContent(chunkData);
            WorldManager.ChunkSenderQueue.Enqueue(chunk);
        }

        private static Task<IBlockState[]> GenerateChunk(AbstractWorld worldIn, Vector2Int origin, HeightMapRouter heightMapRouter)
        {
            var cave = (NoiseGenerator2D)worldIn.WorldGenerator.Noises["Cave"];

            var chunkData = (IBlockState[])_emptyChunk.Clone();
            var noiseValues2DCave = cave.Gen(origin);
            var heightMap = heightMapRouter.GenerateNoiseMap(origin);

            // Define the approximate surface level (adjust this based on your world scale)
            float surfaceLevel = 15f;
            // Define cave thresholds: higher near the surface, lower deeper down
            float surfaceCaveThreshold = cave.Threshold + 0.15f; // Increase the threshold near the surface
            float deepCaveThreshold = cave.Threshold;

            for (var x = 0; x < Chunk.ChunkSize; x++)
            {
                var height = heightMap.GetPoint(x);
                for (var y = 0; y < Math.Clamp(height - origin.y, 0, Chunk.ChunkSize); ++y)
                {
                    var index = x + y * Chunk.ChunkSize;
                    var localY = y + origin.y;

                    // Calculate a dynamic cave threshold based on depth
                    float depthFactor = Mathf.Clamp01(localY / height); // 0 at surface, 1 further down
                    float currentCaveThreshold = Mathf.Lerp(surfaceCaveThreshold, deepCaveThreshold, depthFactor);

                    if (noiseValues2DCave[index] < currentCaveThreshold)
                    {
                        if (localY == height - 1)
                            chunkData[index] = BlockRegistry.BLOCK_GRASS.GetDefaultState();
                        else if (localY > height - 5)
                            chunkData[index] = BlockRegistry.BLOCK_DIRT.GetDefaultState();
                        else
                            chunkData[index] = BlockRegistry.BLOCK_STONE.GetDefaultState();
                    }
                }
            }

            return Task.FromResult(chunkData);
        }
    }
}