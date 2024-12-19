using System;
using Packages.FastNoise2;
using UnityEngine;

namespace World.WorldGeneration.DensityFunctions
{
    public static class DomainWarp
    {
      

        [System.Serializable]
        [DensityFunction]
        public partial class DomainWarpGradient : NoiseFunction
        {
            public FastNoise Source;
            public HybridLookup<FastNoise, float> WarpAmplitude = new HybridLookup<FastNoise, float>(50.0f);
            public float FeatureScale = 100.0f;
        }

        [System.Serializable]
        [DensityFunction]
        public partial class DomainWarpFractalProgressive : NoiseFunction
        {
            public FastNoise DomainWarpSource;
            public HybridLookup<FastNoise, float> Gain = new HybridLookup<FastNoise, float>(0.5f);
            public HybridLookup<FastNoise, float> WeightedStrength = new HybridLookup<FastNoise, float>(0.0f);
            public int Octaves = 3;
            public float Lacunarity = 2.0f;
        }

        [System.Serializable]
        [DensityFunction]
        public partial class DomainWarpFractalIndependant : NoiseFunction
        {
            public FastNoise DomainWarpSource;
            public HybridLookup<FastNoise, float> Gain = new HybridLookup<FastNoise, float>(0.5f);
            public HybridLookup<FastNoise, float> WeightedStrength = new HybridLookup<FastNoise, float>(0.0f);
            public int Octaves = 3;
            public float Lacunarity = 2.0f;
        }
    }
}