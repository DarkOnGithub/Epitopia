using System;
using System.Collections.Generic;
using UnityEngine;
using World.Chunks;

namespace World.WorldGeneration.Noise
{
    public class HeightMap
    {
        private float[][] _noiseMaps;
        private Func<float[], float> _generator;
        private int _amplitude;
        private List<NoiseGenerator1D> _noiseGenerators;
        public HeightMap(Vector2Int origin, Func<float[], float> generator, int amplitude, List<NoiseGenerator1D> noiseGenerators)
        {
            _noiseMaps = new float[noiseGenerators.Count][];
            for (var i = 0; i < noiseGenerators.Count; i++)
                _noiseMaps[i] = noiseGenerators[i].Gen(origin);
            
            _noiseGenerators = noiseGenerators;
            _generator = generator;
            _amplitude = amplitude;
        }

        public int GetPoint(int x)
        {
            var values = new float[_noiseMaps.Length];
            for (var i = 0; i < _noiseMaps.Length; i++)
                values[i] = _noiseGenerators[i].ApplySpline(_noiseMaps[i][x]);
            return (int)Mathf.Round(_generator(values) * _amplitude);
        }
    }

    public class HeightMapRouter : NoiseRouter
    {
        private List<NoiseGenerator1D> _generators = new();
        private int _amplitude;

        public HeightMapRouter(NoiseRouterStruct router, WorldGenerator worldGenerator, int amplitude) : base(router)
        {
            _amplitude = amplitude;
            foreach (var route in router.Routes)
            {
                _generators.Add((NoiseGenerator1D)worldGenerator.Noises[route.Noise]);
            }
        }

        public HeightMap GenerateNoiseMap(Vector2Int origin)
        {
            return new HeightMap(origin, Generator, _amplitude, _generators);
        }
    }
}