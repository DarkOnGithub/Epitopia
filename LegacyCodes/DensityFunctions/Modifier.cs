using System;
using Packages.FastNoise2;
using UnityEngine;

namespace World.WorldGeneration.DensityFunctions
{
    public static class Modifier
    {
        [System.Serializable]
        [DensityFunction]
        public partial class Abs : NoiseFunction
        {
            public FastNoise Source;
        }

        [System.Serializable]
        [DensityFunction]
        public partial class SquareRoot : NoiseFunction
        {
            public FastNoise Source;
        }

        [System.Serializable]
        [DensityFunction]
        public partial class SeedOffset_ : NoiseFunction
        {
            public FastNoise Source;
            public int SeedOffset = 1;
        }

        [System.Serializable]
        [DensityFunction]
        public partial class ConvertRGBA8 : NoiseFunction
        {
            public FastNoise Source;
            public float Min = -1.0f;
            public float Max = 1.0f;
        }

        [System.Serializable]
        [DensityFunction]
        public partial class GeneratorCache : NoiseFunction
        {
            public FastNoise Source;
        }

        [System.Serializable]
        [DensityFunction]
        public partial class Remap : NoiseFunction
        {
            public FastNoise Source;
            public HybridLookup<FastNoise, float> FromMin = new HybridLookup<FastNoise, float>(-1.0f);
            public HybridLookup<FastNoise, float> FromMax = new HybridLookup<FastNoise, float>(1.0f);
            public HybridLookup<FastNoise, float> ToMin = new HybridLookup<FastNoise, float>(0.0f);
            public HybridLookup<FastNoise, float> ToMax = new HybridLookup<FastNoise, float>(1.0f);
        }

        [System.Serializable]
        [DensityFunction]
        public partial class Terrace : NoiseFunction
        {
            public FastNoise Source;
            public float Multiplier = 1.0f;
            public float Smoothness = 0.0f;
        }
    }
}