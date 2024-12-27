using UnityEngine;
using World.Chunks;

namespace World.WorldGeneration.Noise
{
    public class NoiseGenerator2D : NoiseGenerator
    {
        public float Threshold;

        internal NoiseGenerator2D(FastNoise noise, float frequency, float threshold, int seed) : base(
            noise, frequency, seed)
        {
            Threshold = threshold;
        }


        public static NoiseGenerator2D FromJson(Noise2DStruct noiseStruct, float frequency, int seed)
        {
            return new NoiseGenerator2D(FastNoise.FromEncodedNodeTree(noiseStruct.Argument),
                                        frequency, noiseStruct.Threshold, seed);
        }

        public override float[] Gen(Vector2 origin)
        {
            var noiseValues = new float[Chunk.ChunkSizeSquared];
            Noise.GenUniformGrid2D(noiseValues, (int)origin.x, (int)origin.y, Chunk.ChunkSize, Chunk.ChunkSize,
                                   Frequency, Seed);
            return noiseValues;
        }
    }
}