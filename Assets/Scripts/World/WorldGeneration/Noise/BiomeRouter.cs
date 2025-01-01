using System.Collections.Generic;
using UnityEngine;

namespace World.WorldGeneration.Noise
{

    public class BiomeRouterPoint: NoiseRouterPoint<NoiseGenerator1D>
    {
        public BiomeRouterPoint(Vector2 origin, List<NoiseGenerator1D> noiseGenerators) : base(origin, noiseGenerators)
        {
            
        }
        
        public float[] GetBiome(int x)
        {
            var biomeParameters = new float[Generators.Count + 1];
            for (var i = 0; i < Generators.Count; i++)
                biomeParameters[i] = NoiseMaps[i][x];
            return biomeParameters;
        }
    }
    public class BiomeRouter : NoiseRouter<NoiseGenerator1D>
    {
        public BiomeRouter(List<NoiseGenerator1D> noiseGenerators)
        {
            Generators = noiseGenerators;
        }

        public override NoiseRouterPoint<NoiseGenerator1D> GenerateNoiseMap(Vector2Int origin)
        {
            return new BiomeRouterPoint(origin, Generators);
        }
    }
}