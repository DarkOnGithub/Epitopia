using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using World.WorldGeneration.Noise;

namespace World.WorldGeneration.Biomes
{
    public struct BiomeParametersData
    {
        public string Name { get; set; }
        public float[] Temperature { get; set; }
        public float[] Humidity { get; set; }
        public float[] Erosion { get; set; }
        public float[] Continent { get; set; }
    }


    /// <summary>
    /// Noise Order
    /// Continent
    /// Erosion
    /// Temperature
    /// Humidity
    /// </summary>
    public class BiomeProvider
    {
        private Parameters<Biome> _biomesParameters;
        public MultiNoise Noises;
        public Dictionary<string, Biome> Biomes = new Dictionary<string, Biome>();
        public BiomeProvider(BiomeParametersData[] data, INoise erosion, INoise continent)
        {
            var noises = new INoise[5];
            noises[0] = continent;
            noises[1] = erosion;
            
            for (int i = 0; i < 3; i++)
                noises[i + 2] = new NoiseGenerator("FwAAAIC/AACAPwAAAAAAAIA/CQA=", 0.001f);
            
            Noises = new MultiNoise(noises);
            
            var biomeParameters = new List<Tuple<ParamPoint, Func<Biome>>>();
            foreach (var biomeParameter in data)
            {
                var parameter = new ParamPoint(
                    new Param(biomeParameter.Temperature[0], biomeParameter.Temperature[1]),
                    new Param(biomeParameter.Humidity[0], biomeParameter.Humidity[1]),
                    new Param(biomeParameter.Continent[0], biomeParameter.Continent[1]),
                    new Param(biomeParameter.Erosion[0], biomeParameter.Erosion[1])
                );
                var biome = new Biome(biomeParameter.Name);
                biomeParameters.Add(Tuple.Create(parameter, (Func<Biome>)(() => biome)));
                Biomes.Add(biomeParameter.Name, biome);
            }
            _biomesParameters = new Parameters<Biome>(biomeParameters);
            
            
            
        }
        
        public Biome GetBiome(float continent, float erosion, float temperature, float humidity)
        {
            return _biomesParameters.Find(new TargetPoint(temperature, humidity, continent, erosion));
        }
        
        
        public (float continent, float erosion, float temperature, float humidity) ExtractParameters(float[] parameters)
        {
            return (parameters[0], parameters[1], parameters[2], parameters[3]);
        }
    }
}