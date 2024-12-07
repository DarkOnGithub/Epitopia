namespace World.WorldGeneration.DensityFunctions
{
    public static class Blend
    {
        [DensityFunction]
        public struct Add
        {
            public float LHS { get; set; }
            public float RHS { get; set; } 
        }

        [DensityFunction]
        public struct Subtract
        {
            public float LHS { get; set; } 
            public float RHS { get; set; } 
        }

        [DensityFunction]
        public struct Multiply
        {
            public float LHS { get; set; }
            public float RHS { get; set; } 
        }

        [DensityFunction]
        public struct Divide
        {
            public float LHS { get; set; } 
            public float RHS { get; set; } 
        }

        [DensityFunction]
        public struct Min
        {
            public float LHS { get; set; }
            public float RHS { get; set; } 
        }

        [DensityFunction]
        public struct Max
        {
            public float LHS { get; set; }
            public float RHS { get; set; } 
        }

        [DensityFunction]
        public struct MinSmooth
        {
            public float LHS { get; set; }
            public float RHS { get; set; } 
            public float Smoothness { get; set; } 
        }

        [DensityFunction]
        public struct MaxSmooth
        {
            public float LHS { get; set; }
            public float RHS { get; set; } 
            public float Smoothness { get; set; } 
        }

        [DensityFunction]
        public struct PowFloat
        {
            public float Value { get; set; } 
            public float Pow { get; set; } 
        }

        [DensityFunction]
        public struct PowInt
        {
            public float Value { get; set; }
            public int Pow { get; set; } 
        }

        [DensityFunction]
        public struct Fade
        {
            public float A { get; set; }
            public float B { get; set; }
            public float FadeValue { get; set; } 
            public float FadeMin { get; set; } 
            public float FadeMax { get; set; } 
            public string Interpolation { get; set; } 
        }
    }
}