namespace World.WorldGeneration.DensityFunctions
{
    public static class Modifier
    {
          [DensityFunction]
        public struct Abs
        {
            public float Source { get; set; }
        }

        [DensityFunction]
        public struct SquareRoot
        {
            public float Source { get; set; }
        }

        [DensityFunction]
        public struct DomainScale
        {
            public float Source { get; set; }
            public float Scaling { get; set; } 
        }

        [DensityFunction]
        public struct DomainOffset
        {
            public float Source { get; set; }
            public float OffsetX { get; set; } 
            public float OffsetY { get; set; } 
            public float OffsetZ { get; set; } 
            public float OffsetW { get; set; } 
        }

        [DensityFunction]
        public struct DomainRotate
        {
            public float Source { get; set; }
            public float Yaw { get; set; } 
            public float Pitch { get; set; } 
            public float Roll { get; set; } 
        }

        [DensityFunction]
        public struct DomainAxisScale
        {
            public float Source { get; set; }
            public float XScaling { get; set; } 
            public float YScaling { get; set; } 
            public float ZScaling { get; set; } 
            public float WScaling { get; set; } 
        }

        [DensityFunction]
        public struct SeedOffset
        {
            public float Source { get; set; }
            public int SeedOffset__ { get; set; } 
        }

        [DensityFunction]
        public struct ConvertRGBA8
        {
            public float Source { get; set; }
            public float Min { get; set; } 
            public float Max { get; set; } 
        }

        [DensityFunction]
        public struct GeneratorCache
        {
            public float Source { get; set; }
        }

        [DensityFunction]
        public struct Remap
        {
            public float Source { get; set; }
            public float FromMin { get; set; } 
            public float FromMax { get; set; } 
            public float ToMin { get; set; } 
            public float ToMax { get; set; } 
        }

        [DensityFunction]
        public struct Terrace
        {
            public float Source { get; set; }
            public float Multiplier { get; set; } 
            public float Smoothness { get; set; } 
        }

        [DensityFunction]
        public struct AddDimension
        {
            public float Source { get; set; }
            public float NewDimensionPosition { get; set; } 
        }

        [DensityFunction]
        public struct RemoveDimension
        {
            public float Source { get; set; }
            public string RemoveDimension__ { get; set; } 
        }
    }
}