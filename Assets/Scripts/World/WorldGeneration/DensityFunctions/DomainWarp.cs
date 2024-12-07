namespace World.WorldGeneration.DensityFunctions
{
    public static class DomainWarp
    {
        [DensityFunction]
        public struct DomainWarpOpenSimplex
        {
            public float Source { get; set; }
            public float WarpAmplitude { get; set; }
            public float FeatureScale { get; set; } 
        }

        [DensityFunction]
        public struct DomainWarpGradient
        {
            public float Source { get; set; }
            public float WarpAmplitude { get; set; } 
            public float FeatureScale { get; set; } 
        }

        [DensityFunction]
        public struct DomainWarpFractalProgressive
        {
            public float DomainWarpSource { get; set; }
            public float Gain { get; set; } 
            public float WeightedStrength { get; set; } 
            public int Octaves { get; set; } 
            public float Lacunarity { get; set; } 
        }

        [DensityFunction]
        public struct DomainWarpFractalIndependant
        {
            public float DomainWarpSource { get; set; }
            public float Gain { get; set; } 
            public float WeightedStrength { get; set; } 
            public int Octaves { get; set; } 
            public float Lacunarity { get; set; } 
        }
    }
}