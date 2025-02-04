using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using World.Blocks;
using World.Chunks;
using World.WorldGeneration.Biomes;
using World.WorldGeneration.Noise;
using Random = UnityEngine.Random;

namespace World.WorldGeneration.Structures
{
    public class StructurePlacement
    {
        //Unit is chunk
        public const int SuperChunkRadius = 16;
        public const int SuperChunkSize = SuperChunkRadius * Chunk.ChunkSize;
        private NoiseGenerator _noiseGenerator = new("FwAAAIC/AACAPwAAAAAAAIA/BgA=", 0.02f);
        public Dictionary<Biome, List<IStructure>> Structures = new();
        private AbstractWorld _world;
        public StructurePlacement(AbstractWorld world)
        {
            _world = world;
        }
        public void AddStructure(IStructure structure)
        {
            
        }
        
        public void LoadSuperchunk(Vector2Int start)
        {
            var cache = _noiseGenerator.GenerateCache(start, new Vector2Int(SuperChunkSize, 1));
            var biomeProvider = _world.WorldGenerator.BiomeProvider;
            
            for (var x = 0; x < SuperChunkRadius; x++)
            {
                var biomeParametersGenerator =
                    biomeProvider.Noises.GetCache(start, new Vector2Int(1, 1));
                var multiPoint = biomeParametersGenerator.GetPoint(x);
                
                var (continent, erosion, temperature, humidity) = BiomeProvider.ExtractParameters(multiPoint);
                var biome = biomeProvider.GetBiome(continent, erosion, temperature, humidity);
                var noiseValue = cache[x];

                foreach (var structure in Structures[biome])
                {
                    var range = structure.Data.Range;
                    if (noiseValue >= range[0] && noiseValue <= range[1])
                    {
                        
                    }
                }
            }
        }
    }
}