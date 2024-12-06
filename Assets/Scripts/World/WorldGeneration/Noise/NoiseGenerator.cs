using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace World.WorldGeneration.Noise
{
    public class NoiseGenerator
    {
        internal FastNoise Noise;
        public float Frequency;
        public NoiseGenerator(string noiseSource, float frequency)
        {
            Noise = new FastNoise($"{noiseSource}");
            Frequency = frequency;
        }
        internal static NoiseGenerator FromJson(JObject jsonObject)
        {
            Debug.Log(jsonObject);
            return new NoiseGenerator(jsonObject["Argument"].ToString(), (float)jsonObject["Frequency"]);
        }
    }
}