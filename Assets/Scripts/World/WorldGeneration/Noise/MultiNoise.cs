using System.Collections.Generic;
using UnityEngine;
using World.WorldGeneration.DensityFunction;

namespace World.WorldGeneration.Noise
{
    public class MultiNoise
    {
        public INoise[] Noises;


        public MultiNoise(INoise[] noises)
        {
            Noises = noises;
        }

        public NoiseCache<float[]> GetCache(Vector2Int pos, Vector2Int? size = null)
        {
            size ??= new Vector2Int(1, 0);
            var cache = new List<float[]>();
            foreach (var noise in Noises)
                cache.Add(noise.GenerateCache(pos, size.Value));

            return new NoiseCache<float[]>(cache[0], (f, i, _) =>
            {
                var result = new float[cache.Count];
                for (var j = 0; j < cache.Count; j++)
                    result[j] = cache[j][i];
                return result;
            });
        }
    }
}