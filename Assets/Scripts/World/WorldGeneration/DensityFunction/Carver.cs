using System.Linq;
using UnityEngine;
using World.WorldGeneration.Noise;

namespace World.WorldGeneration.DensityFunction
{
    public struct CarverData
    {
        public ThresholdedNoiseGeneratorData caveEntrance { get; set; }
        public ThresholdedNoiseGeneratorData cheeseCave { get; set; }
        public ThresholdedNoiseGeneratorData spaghettiCave { get; set; }
    }
    
    public class Carver
    {
        public static float Median(float[] source)
        {
            var sorted = source.OrderBy(x => x).ToArray();
            int count = sorted.Length;
            if (count % 2 == 0)
            {
                // Even number of elements, average the two middle elements
                return (sorted[count / 2 - 1] + sorted[count / 2]) / 2f;
            }
            else
            {
                // Odd number of elements, return the middle element
                return sorted[count / 2];
            }
        }
        public ThresholdedNoiseGenerator CaveEntrance { get; }
        public ThresholdedNoiseGenerator CheeseCave { get; }
        public ThresholdedNoiseGenerator SpaghettiCave { get; }
        
        public Carver(CarverData data)
        {
            CaveEntrance = new ThresholdedNoiseGenerator(data.caveEntrance);
            CheeseCave = new ThresholdedNoiseGenerator(data.cheeseCave);
            SpaghettiCave = new ThresholdedNoiseGenerator(data.spaghettiCave);
        }
        
        public NoiseCache<bool> CacheDensityPoints(Vector2Int pos)
        {
            var cheeseCache = new NoiseCache<bool>(CheeseCave.GenerateCache(pos), (x, i) => x >= CheeseCave.Data.threshold); 

            var spaghettiCache = new NoiseCache<bool>(SpaghettiCave.GenerateCache(pos), (x, i) =>
            {
                return x >= SpaghettiCave.Data.threshold || cheeseCache.GetPoint(i);
            });
            return spaghettiCache;
        }
    }
}