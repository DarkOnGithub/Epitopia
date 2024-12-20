using System;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Utils;

namespace World.WorldGeneration.Noise
{
    public class NoiseGenerator
    {
        internal FastNoise Noise;
        public float Frequency;
        public int[] Splines;

        internal NoiseGenerator(FastNoise noise, float frequency, int[] splines)
        {
            Noise = noise;
            Frequency = frequency;
            Splines = splines;
        }

        public static NoiseGenerator FromJson(JObject jsonObject)
        {
            return new NoiseGenerator(FastNoise.FromEncodedNodeTree(jsonObject.Get<string>("TreeNode")),
                jsonObject.Get<float>("Frequency"), jsonObject.Get<int[]>("Splines"));
        }
    }
}