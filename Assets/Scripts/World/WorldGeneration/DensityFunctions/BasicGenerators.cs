namespace World.WorldGeneration.DensityFunctions
{
    public static class BasicGenerators
    {
        [DensityFunction]
        public struct Constant
        {
            public float Value { get; set; }
        }

        [DensityFunction]
        public struct White
        {
            public float OutputMin { get; set; }
            public float OutputMax { get; set; }
        }

        [DensityFunction]
        public struct Checkerboard
        {
            public float FeatureScale { get; set; }
            public float OutputMin { get; set; }
            public float OutputMax { get; set; }
        }

        [DensityFunction]
        public struct SineWave
        {
            public float FeatureScale { get; set; }
            public float OutputMin { get; set; }
            public float OutputMax { get; set; }
        }

        [DensityFunction]
        public struct PositionOutput
        {
            public float XMultiplier { get; set; }
            public float YMultiplier { get; set; }
            public float ZMultiplier { get; set; }
            public float WMultiplier { get; set; }
            public float XOffset { get; set; }
            public float YOffset { get; set; }
            public float ZOffset { get; set; }
            public float WOffset { get; set; }
        }

        [DensityFunction]
        public struct DistanceToPoint
        {
            public float PointX { get; set; }
            public float PointY { get; set; }
            public float PointZ { get; set; }
            public float PointW { get; set; }
            public float MinkowskiP { get; set; }
            public string DistanceFunction { get; set; }
        }
    }
}