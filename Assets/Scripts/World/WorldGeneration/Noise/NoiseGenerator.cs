using UnityEngine;
using World.Chunks;

namespace World.WorldGeneration.Noise
{
    public abstract class NoiseGenerator
    {
        public FastNoise Noise;
        public float Frequency;
        public int Seed;

        protected NoiseGenerator(FastNoise noise, float frequency, int seed)
        {
            Seed = seed;
            Noise = noise;
            Frequency = frequency;
        }

        public abstract float[] Gen(Vector2 origin);
    }
}