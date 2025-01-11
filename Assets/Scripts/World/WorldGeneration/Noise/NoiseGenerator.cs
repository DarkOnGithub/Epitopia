using System.Threading;
using UnityEngine;

namespace World.WorldGeneration.Noise
{
    public class NoiseGenerator
    {
        private readonly float _frequency;
        private readonly FastNoise _noise;
        private readonly int _seed;

        public NoiseGenerator(string treeHash, float frequency)
        {
            _noise = FastNoise.FromEncodedNodeTree(treeHash);
            _frequency = frequency;
            _seed = Seed.Next();
        }

        public float[] GenerateCache(Vector2Int pos, Vector2Int size)
        {
            var buffer = new float[size.x * size.y];
            _noise.GenUniformGrid2D(buffer, pos.x, pos.y, size.x, size.y, _frequency, _seed);
            return buffer;
        }
    }
}