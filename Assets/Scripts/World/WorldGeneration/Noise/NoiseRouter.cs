using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using UnityEngine;

namespace World.WorldGeneration.Noise
{

    public abstract class NoiseRouterPoint<T> where T: NoiseGenerator
    {
        protected List<T> Generators = new();
        protected float[][] NoiseMaps;

        public NoiseRouterPoint(Vector2 origin, List<T> noiseGenerators)
        {
            NoiseMaps = new float[noiseGenerators.Count][];
            for (var i = 0; i < noiseGenerators.Count; i++)
                NoiseMaps[i] = noiseGenerators[i].Gen(origin);
            Generators = noiseGenerators;
        }
        
    }
    public abstract class NoiseRouter<T> where T: NoiseGenerator
    {
        protected List<T> Generators = new();
        
        public NoiseRouter()
        {
            
        }

        public abstract NoiseRouterPoint<T> GenerateNoiseMap(Vector2Int origin);

        public void AddGenerator(T generator)
        {
            Generators.Add(generator);
        }



    }
}