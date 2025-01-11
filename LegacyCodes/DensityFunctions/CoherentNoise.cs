using System;
using Packages.FastNoise2;
using UnityEngine;

namespace World.WorldGeneration.DensityFunctions
{
    public static class CoherentNoise
    {
        [System.Serializable]
        [DensityFunction]
        public partial class Simplex : NoiseFunction
        {

           
            public NoiseType Type = NoiseType.Standard;

            public enum NoiseType
            {
                Standard,
                Smooth
            }
        }

        [System.Serializable]
        [DensityFunction]
        public partial class Perlin : NoiseFunction
        {

         
        }

        [System.Serializable]
        [DensityFunction]
        public partial class Value : NoiseFunction
        {

        }

        [System.Serializable]
        [DensityFunction]
        public partial class CellularValue : NoiseFunction
        {
            public HybridLookup<FastNoise, float> MinkowskiP = new HybridLookup<FastNoise, float>(1.5f);
            public HybridLookup<FastNoise, float> JitterModifier = new HybridLookup<FastNoise, float>(1.0f);

           
            public DistanceFunctionType DistanceFunction = DistanceFunctionType.EuclideanSquared;
            public int ValueIndex = 0;

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

        [System.Serializable]
        [DensityFunction]
        public partial class CellularDistance : NoiseFunction
        {
            public HybridLookup<FastNoise, float> MinkowskiP = new HybridLookup<FastNoise, float>(1.5f);
            public HybridLookup<FastNoise, float> JitterModifier = new HybridLookup<FastNoise, float>(1.0f);

            
            public DistanceFunctionType DistanceFunction = DistanceFunctionType.EuclideanSquared;
            public int DistanceIndex0 = 0;
            public int DistanceIndex1 = 1;
            public ReturnType ReturnType = ReturnType.Index0;

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
        public enum ReturnType
        {
            Index0,
            Index0Add1,
            Index0Sub1,
            Index0Mul1,
            Index0Div1
        }
        [System.Serializable]
        [DensityFunction]
        public partial class CellularLookup : NoiseFunction
        {
            public FastNoise Lookup;
            public HybridLookup<FastNoise, float> MinkowskiP = new HybridLookup<FastNoise, float>(1.5f);
            public HybridLookup<FastNoise, float> JitterModifier = new HybridLookup<FastNoise, float>(1.0f);

            public DistanceFunctionType DistanceFunction = DistanceFunctionType.EuclideanSquared;

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