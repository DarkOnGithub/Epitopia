namespace World.WorldGeneration.DensityFunctions
{
    public static class Fractal
    {
        [DensityFunction]
        public struct FractalFBm
        {
            public float Source { get; set; }
            public float Gain { get; set; }
            public float WeightedStrength { get; set; }
            public int Octaves { get; set; } 
            public float Lacunarity { get; set; }
        }

        [DensityFunction]
        public struct FractalPingPong
        {
            public float Source { get; set; }
            public float Gain { get; set; } 
            public float WeightedStrength { get; set; }
            public float PingPongStrength { get; set; }
            public int Octaves { get; set; } 
            public float Lacunarity { get; set; }
        }

        [DensityFunction]
        public struct FractalRidged
        {
            public float Source { get; set; }
            public float Gain { get; set; }
            public float WeightedStrength { get; set; }
            public int Octaves { get; set; }
            public float Lacunarity { get; set; }
        }
    }
}