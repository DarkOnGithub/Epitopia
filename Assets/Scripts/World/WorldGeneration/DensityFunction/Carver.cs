using System.Linq;
using UnityEngine;
using World.Chunks;
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
            var cheeseCache =
                new NoiseCache<bool>(CheeseCave.GenerateCache(pos), (x, i,_) => x >= CheeseCave.Data.threshold);

            var spaghettiCache = new NoiseCache<bool>(SpaghettiCave.GenerateCache(pos),
                                                      (x, i, parameters) =>
                                                      {
                                                          var y = (int)parameters[0];
                                                          var surfaceLevel = (int)parameters[1];
                                                          return x >= SpaghettiCave.Data.threshold ||
                                                                 (cheeseCache.GetPoint(i) && y < surfaceLevel);
                                                      });
            return spaghettiCache;
        }
    }
}