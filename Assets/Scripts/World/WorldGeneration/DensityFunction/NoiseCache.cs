using System;

namespace World.WorldGeneration.DensityFunction
{
    public class NoiseCache<T>
    {
        public readonly float[] _cache;
        private readonly Func<float, int, object[], T> _modifier;

        public NoiseCache(float[] cache, Func<float, int, object[], T> modifier = null)
        {
            modifier ??= (x, i, q) => (T)Convert.ChangeType(x, typeof(T));
            _cache = cache;
            _modifier = modifier;
        }

        public T GetPoint(int index, params object[] args)
        {
            return _modifier(_cache[index], index, args);
        }
    }
}