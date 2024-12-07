namespace World.WorldGeneration.DensityFunctions
{
    public class CoherentNoise
    {
[DensityFunction]
    public struct Simplex
    {
        public float FeatureScale { get; set; }
        public float OutputMin { get; set; }
        public float OutputMax { get; set; }
    }

    [DensityFunction]
    public struct OpenSimplex2
    {
        public float FeatureScale { get; set; }
        public float OutputMin { get; set; }
        public float OutputMax { get; set; }
    }

    [DensityFunction]
    public struct Perlin
    {
        public float FeatureScale { get; set; }
        public float OutputMin { get; set; }
        public float OutputMax { get; set; }
    }

    [DensityFunction]
    public struct Value
    {
        public float FeatureScale { get; set; }
        public float OutputMin { get; set; }
        public float OutputMax { get; set; }
    }

    [DensityFunction]
    public struct CellularValue
    {
        public float MinkowskiP { get; set; }
        public float JitterModifier { get; set; }
        public float FeatureScale { get; set; }
        public float OutputMin { get; set; }
        public float OutputMax { get; set; }
        public string DistanceFunction { get; set; }
        public int ValueIndex { get; set; }
    }

    [DensityFunction]
    public struct CellularDistance
    {
        public float MinkowskiP { get; set; }
        public float JitterModifier { get; set; }
        public float FeatureScale { get; set; }
        public float OutputMin { get; set; }
        public float OutputMax { get; set; }
        public string DistanceFunction { get; set; }
        public int DistanceIndex0 { get; set; }
        public int DistanceIndex1 { get; set; }
        public string ReturnType { get; set; }
    }

    [DensityFunction]
    public struct CellularLookup
    {
        public float Lookup { get; set; }
        public float MinkowskiP { get; set; }
        public float JitterModifier { get; set; }
        public float FeatureScale { get; set; }
        public string DistanceFunction { get; set; }
    }

    [DensityFunction]
    public struct OpenSimplex2S
    {
        public float FeatureScale { get; set; }
        public float OutputMin { get; set; }
        public float OutputMax { get; set; }
    }
    }
}