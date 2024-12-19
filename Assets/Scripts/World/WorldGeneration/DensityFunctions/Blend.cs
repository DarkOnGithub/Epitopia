using System;
using Packages.FastNoise2;
using UnityEngine;

namespace World.WorldGeneration.DensityFunctions
{
    public static class Blends
    {
        [System.Serializable]
        [DensityFunction]
        public partial class Min : NoiseFunction
        {
            public FastNoise LHS;
            public HybridLookup<FastNoise, float> RHS = new HybridLookup<FastNoise, float>(0.0f);
        }

        [System.Serializable]
        [DensityFunction]
        public partial class Max : NoiseFunction
        {
            public FastNoise LHS;
            public HybridLookup<FastNoise, float> RHS = new HybridLookup<FastNoise, float>(0.0f);
        }

        [System.Serializable]
        [DensityFunction]
        public partial class MinSmooth : NoiseFunction
        {
            public FastNoise LHS;
            public HybridLookup<FastNoise, float> RHS = new HybridLookup<FastNoise, float>(0.0f);
            public HybridLookup<FastNoise, float> Smoothness = new HybridLookup<FastNoise, float>(0.1f);
        }

        [System.Serializable]
        [DensityFunction]
        public partial class MaxSmooth : NoiseFunction
        {
            public FastNoise LHS;
            public HybridLookup<FastNoise, float> RHS = new HybridLookup<FastNoise, float>(0.0f);
            public HybridLookup<FastNoise, float> Smoothness = new HybridLookup<FastNoise, float>(0.1f);
        }

        [System.Serializable]
        [DensityFunction]
        public partial class PowFloat : NoiseFunction
        {
            public HybridLookup<FastNoise, float> Value = new HybridLookup<FastNoise, float>(2.0f);
            public HybridLookup<FastNoise, float> Pow = new HybridLookup<FastNoise, float>(2.0f);
        }

        [System.Serializable]
        [DensityFunction]
        public partial class PowInt : NoiseFunction
        {
            public FastNoise Value;
            public int Pow = 2;
        }

        [System.Serializable]
        [DensityFunction]
        public partial class Fade_ : NoiseFunction
        {
            public FastNoise A;
            public FastNoise B;
            public HybridLookup<FastNoise, float> Fade = new HybridLookup<FastNoise, float>(0.0f);
            public HybridLookup<FastNoise, float> FadeMin = new HybridLookup<FastNoise, float>(-1.0f);
            public HybridLookup<FastNoise, float> FadeMax = new HybridLookup<FastNoise, float>(1.0f);
            public InterpolationType Interpolation = InterpolationType.Linear;

            public enum InterpolationType
            {
                Linear,
                Hermite,
                Quintic
            }
        }
    }
}