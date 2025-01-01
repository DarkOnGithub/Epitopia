using System;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Utils;
using World.Chunks;

namespace World.WorldGeneration.Noise
{
    public class NoiseGenerator1D : NoiseGenerator
    {
        public Splines Splines;

        internal NoiseGenerator1D(FastNoise noise, float frequency, float[] splines, int seed) : base(
            noise, frequency, seed)
        {
            Splines = new Splines(splines);
        }

        public float ApplySpline(float point)
        {
            return Splines.ApplySpline(point);
        }

        public static NoiseGenerator1D FromJson(Noise1DStruct noiseStruct, float frequency, int seed)
        {
            return new NoiseGenerator1D(FastNoise.FromEncodedNodeTree(noiseStruct.Argument),
                                        frequency, noiseStruct.Splines, seed);
        }

        public override float[] Gen(Vector2 origin)
        {
            var noiseValues = new float[Chunk.ChunkSize];
            Noise.GenUniformGrid2D(noiseValues, (int)origin.x, 0, Chunk.ChunkSize, 1, Frequency, Seed);
            return noiseValues;
        }
    }
}