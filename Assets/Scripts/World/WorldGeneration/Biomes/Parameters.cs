using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using World.WorldGeneration.Biomes;

namespace World.WorldGeneration.Biomes
{
    public class Param
    {
        public float Min { get; }
        public float Max { get; }

        public Param(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float Distance(Param param)
        {
            var diffMax = param.Min - Max;
            var diffMin = Min - param.Max;
            if (diffMax > 0)
                return diffMax;
            return Math.Max(diffMin, 0);
        }

        public float Distance(float value)
        {
            var diffMax = value - Max;
            var diffMin = Min - value;
            if (diffMax > 0)
                return diffMax;
            return Math.Max(diffMin, 0);
        }

        public Param Union(Param param)
        {
            return new Param(
                Math.Min(Min, param.Min),
                Math.Max(Max, param.Max)
            );
        }
    }

    public class ParamPoint
    {
        public Param Temperature { get; }
        public Param Humidity { get; }
        public Param Continentalness { get; }
        public Param Erosion { get; }


        public ParamPoint(
            Param temperature,
            Param humidity,
            Param continentalness,
            Param erosion
        )
        {
            Temperature = temperature;
            Humidity = humidity;
            Continentalness = continentalness;
            Erosion = erosion;
        }

        public float Fitness(ParamPoint point)
        {
            return Mathf.Pow((float)Temperature.Distance(point.Temperature), 2) +
                   Mathf.Pow((float)Humidity.Distance(point.Humidity), 2) +
                   Mathf.Pow((float)Continentalness.Distance(point.Continentalness), 2) +
                   Mathf.Pow((float)Erosion.Distance(point.Erosion), 2);
        }

        public List<Param> Space()
        {
            return new List<Param>
                   {
                       Temperature, Humidity, Continentalness, Erosion
                   };
        }
    }

    public class TargetPoint
    {
        public float Temperature { get; }
        public float Humidity { get; }
        public float Continentalness { get; }
        public float Erosion { get; }

        public TargetPoint(float temperature, float humidity, float continentalness, float erosion)
        {
            Temperature = temperature;
            Humidity = humidity;
            Continentalness = continentalness;
            Erosion = erosion;
        }


        public List<float> ToArray()
        {
            return new List<float>
                   {
                       Temperature, Humidity, Continentalness, Erosion
                   };
        }
    }

    public class Parameters<T>
    {
        private readonly RTree<T> index;

        public Parameters(List<Tuple<ParamPoint, Func<T>>> things)
        {
            index = new RTree<T>(things);
        }

        public T Find(TargetPoint target)
        {
            return index.Search(target);
        }
    }
}