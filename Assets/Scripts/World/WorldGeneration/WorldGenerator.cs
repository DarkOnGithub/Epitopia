using System.Threading.Tasks;
using Network.Server;
using UnityEngine;
using Utils;
using World.Blocks;
using World.Chunks;
using World.WorldGeneration.DensityFunction;

namespace World.WorldGeneration
{
    public class WorldGenerator
    {
        private readonly IBlockState[] _emptyChunk = new IBlockState[Chunk.ChunkSizeSquared];
        private readonly HeightMap _heightMap;
        private readonly Carver _carver;
        
        public string DefaultPath = Server.Instance.ConfigDirectory + "/WorldGen/";

        public AbstractWorld WorldIn;


        public WorldGenerator(AbstractWorld world)
        {
            WorldIn = world;

            for (var i = 0; i < Chunk.ChunkSizeSquared; i++)
                _emptyChunk[i] = BlockRegistry.BLOCK_AIR.GetDefaultState();

            _heightMap = new HeightMap(JsonUtils.LoadJson<HeightMapData>(DensityFunctionsPath + "HeightMap.json"));
            _carver = new Carver(JsonUtils.LoadJson<CarverData>(DensityFunctionsPath + "Carver.json"));

        }

        public string DensityFunctionsPath => DefaultPath + "DensityFunctions/";
        public string BiomesPath => DefaultPath + "Biomes/";

        public async Task GenerateChunk(Chunk chunk)
        {
            var chunkData = await FillChunk(chunk.Origin);
            chunk.UpdateContent(chunkData);
            WorldManager.ChunkSenderQueue.Enqueue(chunk);
        }

        private Task<IBlockState[]> FillChunk(Vector2Int origin)
        {
            var heightMapCache = _heightMap.CacheHeight(origin);
            var carverCache = _carver.CacheDensityPoints(origin);
            var emptyChunk = (IBlockState[])_emptyChunk.Clone();
            for (var x = 0; x < Chunk.ChunkSize; x++)
            {
                var surfaceLevel = (int)heightMapCache.GetPoint(x);
                for (var y = 0; y < Mathf.Clamp(surfaceLevel - origin.y, 0, Chunk.ChunkSize); y++)
                {
                    var index = x + y * Chunk.ChunkSize;
                    var localY = y + origin.y;
                    if (!carverCache.GetPoint(index))
                    {
                        if (localY == surfaceLevel - 1)
                            emptyChunk[index] = BlockRegistry.BLOCK_GRASS.GetDefaultState();
                        else if (localY > surfaceLevel - 15)
                            emptyChunk[index] = BlockRegistry.BLOCK_DIRT.GetDefaultState();
                        else
                            emptyChunk[index] = BlockRegistry.BLOCK_STONE.GetDefaultState();
                    } else{
                        if(localY == surfaceLevel - 1)
                            continue;
                        if(localY > surfaceLevel - 14)
                            emptyChunk[index] = BlockRegistry.WALL_DIRT.GetDefaultState();
                        else
                            emptyChunk[index] = BlockRegistry.WALL_STONE.GetDefaultState();
                    }
                 
                   
                }
            }

            return Task.FromResult(emptyChunk);
        }
    }
}