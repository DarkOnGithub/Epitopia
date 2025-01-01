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
            var chunkData = await GenerateChunk(worldIn, chunk.Origin, worldIn.WorldGenerator);
            chunk.UpdateContent(chunkData);
            WorldManager.ChunkSenderQueue.Enqueue(chunk);
        }

        private static Task<IBlockState[]> GenerateChunk(AbstractWorld worldIn, Vector2Int origin, WorldGenerator worldGenerator)
        {
            var cave = (NoiseGenerator2D)worldIn.WorldGenerator.Noises["Cave"];

            var chunkData = (IBlockState[])_emptyChunk.Clone();
            var noiseValues2DCave = cave.Gen(origin);
            var heightMap = (HeightMap)worldGenerator.HeightMapRouter.GenerateNoiseMap(origin);
            var biomeRouter = worldGenerator.BiomeRouter;

            var biomeGenerator = (BiomeRouterPoint)biomeRouter.GenerateNoiseMap(origin);



            for (var x = 0; x < Chunk.ChunkSize; x++)
            {
                var surfaceLevel = heightMap.GetPoint(x);
                var biomeParameters = biomeGenerator.GetBiome(x);   
                var elevation = MathUtils.ScaledTanh(surfaceLevel, worldGenerator.ElevationFactor);
                var biome = worldGenerator.BiomeFinder.GetBiome(biomeParameters[0], biomeParameters[1], biomeParameters[2], elevation);
                var surfaceRule = biome.SurfaceRule;
                for (var y = 0; y < Math.Clamp(surfaceLevel - origin.y, 0, Chunk.ChunkSize); ++y)
                {
                    var index = x + y * Chunk.ChunkSize;
                    var localY = y + origin.y;
                    if (noiseValues2DCave[index] < cave.Threshold)
                    {
                        var block = surfaceRule.GetRule(surfaceLevel, localY).GetBlock();
                        chunkData[index] = block;
                    }
                }
            }
            return Task.FromResult(chunkData);
        }
    }
}