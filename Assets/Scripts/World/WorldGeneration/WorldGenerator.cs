using System;
using System.Collections.Generic;
using System.Linq;
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
        public readonly HeightMap HeightMap;
        private readonly Carver _carver;
        public readonly BiomeProvider BiomeProvider;

        public string DefaultPath => Server.Instance.ConfigDirectory + "/WorldGen/";
        public string DensityFunctionsPath => DefaultPath + "DensityFunctions/";
        public string WorldSettingsPath => DefaultPath + "WorldSettings/";
        private readonly NoiseGenerator _vegetationNoise = new("FwAAAIC/AACAPwAAAAAAAIA/BgA=", 0.07f);
        
        public AbstractWorld WorldIn;
        
        public WorldGenerator(AbstractWorld world)
        {
            WorldIn = world;
            
            HeightMap = new HeightMap(JsonUtils.LoadJson<HeightMapData>(DensityFunctionsPath + "HeightMap.json"));
            _carver = new Carver(JsonUtils.LoadJson<CarverData>(DensityFunctionsPath + "Carver.json"));
            var worldSettings = JsonUtils.LoadJson<WorldSettingsData>(WorldSettingsPath + world.Identifier + ".json");
            BiomeProvider = new BiomeProvider(worldSettings.BiomeSources, HeightMap.ContinentNoise,
                                              HeightMap.ErosionNoise);
        }
        
        public int GetHeightAt(int x) => (int)HeightMap.CacheHeight(
                new Vector2Int(x, 0), new Vector2Int(1, 1)
            ).GetPoint(x);
        

        public async Task GenerateChunk(Chunk chunk)
        {
            var position = chunk.Position;
            var heightMapCache = HeightMap.CacheHeight(position);
            var carverCache = _carver.CacheDensityPoints(position);
            
            var chunkData = chunk.BlockStatesRef;
            var biomeParametersGenerator =
                BiomeProvider.Noises.GetCache(position, new Vector2Int(Chunk.ChunkSize, Chunk.ChunkSize));
            
            var vegetationCache = _vegetationNoise.GenerateCache(position, new Vector2Int(Chunk.ChunkSize, 1));
            
            //Density and Biomes
            for (var x = 0; x < Chunk.ChunkSize; x++)
            {
                var surfaceLevel = (int)heightMapCache.GetPoint(x);
                var multiPoint = biomeParametersGenerator.GetPoint(x);
                
                var (continent, erosion, temperature, humidity) = BiomeProvider.ExtractParameters(multiPoint);
                var biome = BiomeProvider.GetBiome(continent, erosion, temperature, humidity);
                var lastLayerDepth = surfaceLevel - biome.SurfaceRules.Depth;
                int maxHeight = Mathf.Clamp(surfaceLevel - position.y, 0, Chunk.ChunkSize);

                for (int y = 0; y < maxHeight; y++)
                {
                    int index = (x, y).ToIndex();
                    int localY = y + position.y;
                    if (localY > surfaceLevel)
                        continue;
            
                    if (!carverCache.GetPoint(index, localY, lastLayerDepth))
                        chunkData[index] = biome.SurfaceRules.GetRule(surfaceLevel - localY)();
                    else
                        chunkData[index] = BlockRegistry.BlockAir.CreateBlockState();
                    if (localY >= surfaceLevel - 2)
                        continue;
                    chunkData[index].WallId = localY > surfaceLevel - 14 ? (byte)1 : (byte)2;
                    
                 
                }
                // if (position.y <= surfaceLevel && surfaceLevel < position.y + Chunk.ChunkSize)
                //     biome.Vegetation.GenerateVegetation(chunkData, vegetationCache[x],
                //                                         new Vector2Int(x, surfaceLevel - position.y), position);
            }

            //
            // for (int x = 0; x < Chunk.ChunkSize; x++)
            // {
            //     int surfaceLevel = surfaceLevels[x];
            //
            //     for (int y = 0; y < Chunk.ChunkSize; y++)
            //     {
            //         int index = x + y * Chunk.ChunkSize;
            //
            //         var block = chunkData[index];
            //         if(block == null)
            //             block = chunkData[index] = BlockRegistry.BLOCK_AIR.GetDefaultState();
            //         int localY = y + origin.y;
            //         int lightLevel = 0;
            //
            //         if (localY >= surfaceLevel)
            //             lightLevel = 15;
            //         else
            //         {
            //             if(block.Properties.IsSolid)
            //                 lightLevel = Mathf.Max(0, 15 - (surfaceLevel - localY) * 2);
            //             else
            //                 lightLevel = Mathf.Max(0, 15 - (surfaceLevel - localY));
            //         }
            //         
            //         chunkData[index].LightLevel = (byte)lightLevel;
            //     }
            // }mmm
        }
    }
}