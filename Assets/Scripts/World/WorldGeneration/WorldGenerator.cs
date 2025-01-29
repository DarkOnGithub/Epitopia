using System.Collections.Generic;
using System.Threading.Tasks;
using Network.Server;
using UnityEngine;
using Utils;
using World.Blocks;
using World.Chunks;
using World.WorldGeneration.Biomes;
using World.WorldGeneration.DensityFunction;
using World.WorldGeneration.Noise;

namespace World.WorldGeneration
{
    public struct WorldSettingsData
    {
        public BiomeParametersData[] BiomeSources { get; set; }
    }

    public class WorldGenerator
    {
        private readonly IBlockState[] _emptyChunk = new IBlockState[Chunk.ChunkSizeSquared];
        private readonly HeightMap _heightMap;
        private readonly Carver _carver;
        public readonly BiomeProvider BiomeProvider;

        public string DefaultPath = Server.Instance.ConfigDirectory + "/WorldGen/";
        public string DensityFunctionsPath => DefaultPath + "DensityFunctions/";
        public string WorldSettingsPath => DefaultPath + "WorldSettings/";
        private NoiseGenerator _vegetationNoise = new("FwAAAIC/AACAPwAAAAAAAIA/BgA=", 0.07f);

        public const int PreloadDistance = Chunk.ChunkSize * 16;

        public AbstractWorld WorldIn;


        public WorldGenerator(AbstractWorld world)
        {
            WorldIn = world;

            for (var i = 0; i < Chunk.ChunkSizeSquared; i++)
                _emptyChunk[i] = BlockRegistry.BLOCK_AIR.GetDefaultState();

            _heightMap = new HeightMap(JsonUtils.LoadJson<HeightMapData>(DensityFunctionsPath + "HeightMap.json"));
            _carver = new Carver(JsonUtils.LoadJson<CarverData>(DensityFunctionsPath + "Carver.json"));
            var worldSettings = JsonUtils.LoadJson<WorldSettingsData>(WorldSettingsPath + world.Identifier + ".json");
            BiomeProvider = new BiomeProvider(worldSettings.BiomeSources, _heightMap.ContinentNoise,
                                              _heightMap.ErosionNoise);
        }

        public async Task GenerateChunk(Chunk chunk)
        {
            var chunkData = await FillChunk(chunk.Origin);
            chunk.UpdateContent(chunkData);
            WorldManager.ChunkSenderQueue.Enqueue(chunk);
        }

        public int GetHeightAt(int x)
        {
            return (int)_heightMap.CacheHeight(new Vector2Int(x, 0), new Vector2Int(1, 1)).GetPoint(x);
        }

        private Task<IBlockState[]> FillChunk(Vector2Int origin)
        {
            try
            {

                var heightMapCache = _heightMap.CacheHeight(origin);
                var carverCache = _carver.CacheDensityPoints(origin);
                
                var chunkData = (IBlockState[])_emptyChunk.Clone();
                var biomeParametersGenerator =
                    BiomeProvider.Noises.GetCache(origin, new Vector2Int(Chunk.ChunkSize, Chunk.ChunkSize));
                
                var vegetationCache = _vegetationNoise.GenerateCache(origin, new Vector2Int(Chunk.ChunkSize, 1));

                //Density and Biomes

                for (var x = 0; x < Chunk.ChunkSize; x++)
                {
                    var surfaceLevel = (int)heightMapCache.GetPoint(x);

                    var multiPoint = biomeParametersGenerator.GetPoint(x);
                    
                    var (continent, erosion, temperature, humidity) = BiomeProvider.ExtractParameters(multiPoint);
                    var biome = BiomeProvider.GetBiome(continent, erosion, temperature, humidity);
                    var lastLayerDepth = surfaceLevel - biome.SurfaceRules.Depth;
                    
                    for (var y = 0; y < Mathf.Clamp(surfaceLevel - origin.y, 0, Chunk.ChunkSize); y++)
                    {
                        var index = x + y * Chunk.ChunkSize;
                        var localY = y + origin.y;
                        
                        if (!carverCache.GetPoint(index, localY, lastLayerDepth))
                        {
                            chunkData[index] = biome.SurfaceRules.GetRule(surfaceLevel - localY)();
                        }
                        else
                        {
                            if (localY == surfaceLevel - 1)
                                continue;
                            if (localY > surfaceLevel - 14)
                                chunkData[index] = BlockRegistry.WALL_DIRT.GetDefaultState();
                            else
                                chunkData[index] = BlockRegistry.WALL_STONE.GetDefaultState();
                        }
                       
                    }
                    if (origin.y <= surfaceLevel && surfaceLevel < origin.y + Chunk.ChunkSize)
                        biome.Vegetation.GenerateVegetation(chunkData, vegetationCache[x], new Vector2Int(x, surfaceLevel - origin.y));
                }


                return Task.FromResult(chunkData);
            }
            catch (System.Exception e)
            {
                Debug.LogError(origin + " " + e);
                return null;
            }

        }
    }
}