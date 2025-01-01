using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using World.Chunks;

namespace World.WorldGeneration.Noise
{
    public struct Router
    {
        public float Weight;
        public string Noise;
    }

    public struct NoiseRouterStruct
    {
        public string Type;
        public string Method;
        public Router[] Routes;
    }


    public class HeightMap : NoiseRouterPoint<NoiseGenerator1D>
    {
        private Func<float[], float> _generator;
        private int _amplitude;
        public HeightMap(Vector2Int origin, Func<float[], float> generator, int amplitude, List<NoiseGenerator1D> noiseGenerators) : base(origin, noiseGenerators)
        {
            _generator = generator;
            _amplitude = amplitude;
        }

        public int GetPoint(int x)
        {
            var values = new float[NoiseMaps.Length];
            for (var i = 0; i < NoiseMaps.Length; i++)
                values[i] = Generators[i].ApplySpline(NoiseMaps[i][x]);
            return (int)Mathf.Round(_generator(values) * _amplitude);
        }
    }

    public class HeightMapRouter : NoiseRouter<NoiseGenerator1D>
    {
        private int _amplitude;
        protected Func<float[], float> Generator;

        public Func<float[], float> Blend(NoiseRouterStruct router)
        {
            switch (router.Method)
            {
                case "Add":
                    return (values) => { return values.Sum(); };
                case "WeightedAddition":
                    return (values) => { return values.Select((t, i) => t * router.Routes[i].Weight).Sum(); };
                case "Lerp":
                    var totalWeight = router.Routes.Sum(r => r.Weight);
                    return (values) =>
                    {
                        return values.Select((t, i) => t * router.Routes[i].Weight).Sum() / totalWeight;
                    };
                case "Multiply":
                    return (values) => { return values.Aggregate((a, b) => a * b); };
                default:
                    return (_) => 0f;
            }
        }


        public HeightMapRouter(NoiseRouterStruct router, WorldGenerator worldGenerator, int amplitude) : base()
        {
            switch (router.Type)
            {
                case "Blend":
                    Generator = Blend(router);
                    break;
            }
            _amplitude = amplitude;
            foreach (var route in router.Routes)
                AddGenerator((NoiseGenerator1D)worldGenerator.Noises[route.Noise]);
        }

        public override NoiseRouterPoint<NoiseGenerator1D> GenerateNoiseMap(Vector2Int origin)
        {
            return new HeightMap(origin, Generator, _amplitude, Generators);
        }
    }
}