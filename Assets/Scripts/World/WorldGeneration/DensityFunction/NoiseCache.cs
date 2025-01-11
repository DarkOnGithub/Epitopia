using System;

namespace World.WorldGeneration.DensityFunction
{
    public class NoiseCache<T>
    {
        private readonly float[] _cache;
        private readonly Func<float, int, T> _modifier;

        public NoiseCache(float[] cache, Func<float, int, T> modifier = null)
        {
            modifier ??= (x, i) => (T)Convert.ChangeType(x, typeof(T));
            _cache = cache;
            _modifier = modifier;
        }

        public T GetPoint(int index)
        {
            return _modifier(_cache[index], index);
        }
    }
}