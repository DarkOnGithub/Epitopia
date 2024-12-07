using System;
using Newtonsoft.Json.Linq;
using UnityEngine;
using World.WorldGeneration.WorldDataParser;

namespace World.WorldGeneration.Noise
{
    public class NoiseGenerator
    {
        internal FastNoise Noise;
        public float Frequency;
        internal NoiseGenerator(FastNoise noise)
        {
            Noise = noise;
        }
        // internal static NoiseGenerator FromJson(JObject jsonObject)
        // {
        //     //return new FastNoise(jsonObject.Get<float>("Frequency"));
        // }
    }
}