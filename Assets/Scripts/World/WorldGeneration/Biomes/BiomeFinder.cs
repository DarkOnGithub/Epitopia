using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;
using Utils;

namespace World.WorldGeneration.Biomes
{
    public class BiomeFinder
    {
        public List<Biome> Biomes = new();
        private BetterLogger _logger = new BetterLogger(typeof(BiomeFinder));
        
        public BiomeFinder()
        {
            
        }
        
        public void AddBiome(Biome biome)
        {
            Biomes.Add(biome);
        }
        
        public Biome GetBiome(float temperature, float humidity, float vegetation, float elevation)
        {
            var matchingBiomes = FindMatchingBiomes(temperature, humidity, vegetation, elevation);

            if (matchingBiomes.Any())
            {
                return matchingBiomes.First();
            }

            return FindClosestBiome(temperature, humidity, vegetation, elevation);
        }

        private List<Biome> FindMatchingBiomes(float temperature, float humidity, float vegetation, float elevation)
        {
            return Biomes.Where(biome =>
                IsWithinRange(temperature, biome.BiomeDescriptor.Temperature) &&
                IsWithinRange(humidity, biome.BiomeDescriptor.Humidity) &&
                IsWithinRange(vegetation, biome.BiomeDescriptor.Vegetation) &&
                IsWithinRange(elevation, biome.BiomeDescriptor.Elevation))
                .ToList();
        }

        private Biome FindClosestBiome(float temperature, float humidity, float vegetation, float elevation)
        {
            return Biomes
                .OrderBy(biome => CalculateDistance(temperature, humidity, vegetation, elevation, biome))
                .FirstOrDefault();
        }

        private float CalculateDistance(float temperature, float humidity, float vegetation, float elevation, Biome biome)
        {
            return Mathf.Sqrt(
                Mathf.Pow(CalculateNormalizedDifference(temperature, biome.BiomeDescriptor.Temperature), 2) +
                Mathf.Pow(CalculateNormalizedDifference(humidity, biome.BiomeDescriptor.Elevation), 2) +
                Mathf.Pow(CalculateNormalizedDifference(vegetation, biome.BiomeDescriptor.Vegetation), 2) +
                Mathf.Pow(CalculateNormalizedDifference(elevation, biome.BiomeDescriptor.Humidity), 2) 
            );
        }

        private float CalculateNormalizedDifference(float value, Range range)
        {
            if (Mathf.Abs(range.Max - range.Min) <= 0.01f)
                return 0;

            float midPoint = (range.Min + range.Max) / 2;
            float rangeSize = (range.Max - range.Min) / 2;
            return Mathf.Abs(value - midPoint) / rangeSize;
        }

        private bool IsWithinRange(float value, Range range)
        {
            return value >= range.Min && value <= range.Max;
        }

       
    }
}
