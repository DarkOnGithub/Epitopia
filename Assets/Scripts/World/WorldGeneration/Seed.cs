using System;

namespace World.WorldGeneration
{
    public static class Seed
    {
        public static int SeedValue;
        private static Random _random;

        public static void Initialize(int? seed = null)
        {
            seed = seed ?? new Random().Next();
            _random = new Random(seed.Value);
            SeedValue = seed.Value;
        }

        public static int Next()
        {
            return _random.Next();
        }
    }
}