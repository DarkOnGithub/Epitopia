using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

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

    public abstract class NoiseRouter
    {
        protected Func<float[], float> Generator;

        public Func<float[], float> Blend(NoiseRouterStruct router)
        {
            var noise = new List<NoiseGenerator>();
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

        public NoiseRouter(NoiseRouterStruct router)
        {
            switch (router.Type)
            {
                case "Blend":
                    Generator = Blend(router);
                    break;
            }
        }
    }
}