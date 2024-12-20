using System;
using System.Threading.Tasks;
using UnityEngine;
using World.Blocks;
using World.Chunks;
using Random = System.Random;

namespace World.WorldGeneration
{
    public static class WorldGeneration
    {
        private static IBlockState[] _emptyChunk = new IBlockState[Chunk.ChunkSizeSquared];
        private static int _seed = new Random().Next(-2, 232109);

        static WorldGeneration()
        {
            for (var i = 0; i < Chunk.ChunkSizeSquared; i++) _emptyChunk[i] = BlockRegistry.BLOCK_AIR.GetDefaultState();
        }

        public static async Task GenerateChunk(AbstractWorld worldIn, Chunk chunk)
        {
            var chunkData = await GenerateChunk(worldIn, chunk.Origin);
            chunk.UpdateContent(chunkData);
            WorldManager.ChunkSenderQueue.Enqueue(chunk);
        }

        private static Task<IBlockState[]> GenerateChunk(AbstractWorld worldIn, Vector2Int origin)
        {
            var chunkData = (IBlockState[])_emptyChunk.Clone();
            //var random = new System.Random().Next(-2,2);
            for (var x = 0; x < Chunk.ChunkSize; x++)
            for (var y = 0; y < Chunk.ChunkSize; y++)
            {
                var noise = Mathf.PerlinNoise((x + origin.x + _seed) * 0.1f, (y + origin.y + _seed) * 0.1f);
                var height = Mathf.FloorToInt(noise * Chunk.ChunkSize);

                if (y + origin.y < height)
                    chunkData[x + y * Chunk.ChunkSize] = BlockRegistry.BLOCK_DIRT.GetDefaultState();
            }

            return Task.FromResult(chunkData);
        }
    }
}