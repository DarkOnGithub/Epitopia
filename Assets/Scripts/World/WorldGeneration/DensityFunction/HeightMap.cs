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
        public SplinedNoiseGeneratorData[] noiseEntries { get; set; }
        public float[] weightFactors { get; set; }
        public SplinedNoiseGeneratorData[] detailEntries { get; set; }
    }


    public class HeightMap
    {
        private static Vector2Int _size = new(Chunk.ChunkSize, 1);
        private readonly SplinedNoiseGenerator[] _detailsGenerators;
        private readonly SplinedNoiseGenerator[] _noiseGenerators;
        public HeightMapData Data { get; }

        public HeightMap(HeightMapData data)
        {
            Data = data;
            foreach (var entry in data.noiseEntries)
            {
                _noiseGenerators = new SplinedNoiseGenerator[data.noiseEntries.Length];

                for (var i = 0; i < data.noiseEntries.Length; i++)
                    _noiseGenerators[i] = new SplinedNoiseGenerator(entry, data.yOffset);
            }

            foreach (var entry in data.detailEntries)
            {
                _detailsGenerators = new SplinedNoiseGenerator[data.detailEntries.Length];

                for (var i = 0; i < data.detailEntries.Length; i++)
                    _detailsGenerators[i] = new SplinedNoiseGenerator(entry, data.yOffset);
            }
        }


        public NoiseCache<float>  CacheHeight(Vector2Int origin, Vector2Int? size = null)
        {
            size ??= _size;
            var finalCache = _noiseGenerators[0].GenerateCache(origin, size.Value);
            var j = 0;
            foreach (var generator in _noiseGenerators.Skip(1))
            {
                var layer = generator.GenerateCache(origin, size.Value);
                for (var i = 0; i < finalCache.Length; i++)
                    finalCache[i] = Mathf.Lerp(finalCache[i], layer[i], Data.weightFactors[j]);
                j++;
            }

            var detailCache = _detailsGenerators[0].GenerateCache(origin, size.Value);

            return new NoiseCache<float>(finalCache, (x, i) =>
            {
                var surfaceLevel = x * Data.higherAmplitude;
                var detailLevel = detailCache[i] * Data.lowerAmplitude;
                return surfaceLevel + detailLevel;
            });
        }
    }
}