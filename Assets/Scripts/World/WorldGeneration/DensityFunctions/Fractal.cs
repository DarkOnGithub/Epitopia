using System;
using Packages.FastNoise2;
using UnityEngine;

namespace World.WorldGeneration.DensityFunctions
{
    public static class Fractal
    {
        [System.Serializable]
        [DensityFunction]
        public partial class FractalFBm : NoiseFunction
        {
            public FastNoise Source;
            public HybridLookup<FastNoise, float> Gain = new HybridLookup<FastNoise, float>(0.5f);
            public HybridLookup<FastNoise, float> WeightedStrength = new HybridLookup<FastNoise, float>(0.0f);
            public int Octaves = 3;
            public float Lacunarity = 2.0f;
        }

        [System.Serializable]
        [DensityFunction]
        public partial class FractalPingPong : NoiseFunction
        {
            public FastNoise Source;
            public HybridLookup<FastNoise, float> Gain = new HybridLookup<FastNoise, float>(0.5f);
            public HybridLookup<FastNoise, float> WeightedStrength = new HybridLookup<FastNoise, float>(0.0f);
            public HybridLookup<FastNoise, float> PingPongStrength = new HybridLookup<FastNoise, float>(2.0f);
            public int Octaves = 3;
            public float Lacunarity = 2.0f;
        }

        [System.Serializable]
        [DensityFunction]
        public partial class FractalRidged : NoiseFunction
        {
            public FastNoise Source;
            public HybridLookup<FastNoise, float> Gain = new HybridLookup<FastNoise, float>(0.5f);
            public HybridLookup<FastNoise, float> WeightedStrength = new HybridLookup<FastNoise, float>(0.0f);
            public int Octaves = 3;
            public float Lacunarity = 2.0f;
        }
    }
}