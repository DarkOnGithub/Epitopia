using System.Linq;
using UnityEngine;
using World.Chunks;
using World.WorldGeneration.Noise;

namespace World.WorldGeneration.DensityFunction
{
    public class HeightMapData
    {
        public int higherAmplitude { get; set; }
        public int lowerAmplitude { get; set; }

        public int yOffset { get; set; }
        public SplinedNoiseGeneratorData erosion { get; set; }
        public SplinedNoiseGeneratorData continent { get; set; }
        public float blendFactor { get; set; }
        public SplinedNoiseGeneratorData detail { get; set; }
    }


    public class HeightMap
    {
        private static Vector2Int _size = new(Chunk.ChunkSize, 1);

        public readonly SplinedNoiseGenerator ErosionNoise;
        public readonly SplinedNoiseGenerator ContinentNoise;
        
        public SplinedNoiseGenerator DetailNoise;

        public HeightMapData Data { get; }

        public HeightMap(HeightMapData data)
        {
            Data = data;

            ErosionNoise = new SplinedNoiseGenerator(data.erosion, data.yOffset);
            ContinentNoise = new SplinedNoiseGenerator(data.continent, data.yOffset);
            
            DetailNoise = new SplinedNoiseGenerator(data.detail, data.yOffset);

        }


        public NoiseCache<float> CacheHeight(Vector2Int origin, Vector2Int? size = null)
        {
            size ??= _size;

            var erosionCache = ErosionNoise.GenerateCache(origin, size.Value);
            var continentCache = ContinentNoise.GenerateCache(origin, size.Value);
            var finalCache = new float[erosionCache.Length];
            for (int i = 0; i < finalCache.Length; i++)
                finalCache[i] = continentCache[i] - erosionCache[i];


            var detailCache = DetailNoise.GenerateCache(origin, size.Value);

            return new NoiseCache<float>(finalCache, (x, i, _) =>
            {
                var surfaceLevel = x * Data.higherAmplitude;
                var detailLevel = detailCache[i] * Data.lowerAmplitude;
                return (surfaceLevel + detailLevel) - (int)(Data.higherAmplitude * 0.12);
            });
        }
    }
}