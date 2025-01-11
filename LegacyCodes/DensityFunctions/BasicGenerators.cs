using System;
using Packages.FastNoise2;
using Unity.Collections;
using UnityEngine;

namespace World.WorldGeneration.DensityFunctions
{
    public static class BasicGenerators
    {
        [System.Serializable]
        [DensityFunction]
        public partial class Constant : NoiseFunction
        {
            [ReadOnly]
            public float Value = 1.0f;
        }

        [System.Serializable]
        [DensityFunction]
        public partial class White : NoiseFunction
        {

        }

        [System.Serializable]
        [DensityFunction]
        public partial class Checkerboard : NoiseFunction
        {
          
        }

        [System.Serializable]
        [DensityFunction]
        public partial class SineWave : NoiseFunction
        {
          
        }

        [System.Serializable]
        [DensityFunction]
        public partial class PositionOutput : NoiseFunction
        {
            [ReadOnly]
            public float XOffset = 0.0f;
            [ReadOnly]
            public float YOffset = 0.0f;
            [ReadOnly]
            public float ZOffset = 0f;
            [ReadOnly]
            public float WOffset = 0.0f;
            [ReadOnly]
            public float XMultiplier = 0.0f;
            [ReadOnly]
            public float YMultiplier = 0.0f;
            [ReadOnly]
            public float ZMultiplier = 0.0f;
            [ReadOnly]
            public float WMultiplier = 0.0f;
        }

        [System.Serializable]
        [DensityFunction]
        public partial class DistanceToPoint : NoiseFunction
        {
            [ReadOnly]
            public float PointX = (0.0f);
            [ReadOnly]
            public float PointY = (0.0f);
            [ReadOnly]
            public float PointZ = (0.0f);
            [ReadOnly]
            public float PointW = (0.0f);
            [ReadOnly]
            public float MinkowskiP = (1.5f);
            [ReadOnly]
            public DistanceFunctionType DistanceFunction = DistanceFunctionType.Euclidean;

            public enum DistanceFunctionType
            {
                Euclidean,
                EuclideanSquared,
                Manhattan,
                Hybrid,
                MaxAxis,
                Minkowski
            }
        }
    }
}