using Newtonsoft.Json;
using UnityEngine;
using World.Chunks;

namespace World.WorldGeneration.Noise
{
    public struct SplinedNoiseGeneratorData
    {
        [JsonProperty] public string noise { get; set; }

        [JsonProperty] public float frequency { get; set; }

        [JsonProperty] public float[] spline { get; set; }
    }

    public class SplinedNoiseGenerator : INoise
    {
        private static readonly Vector2Int _size = new(Chunk.ChunkSize, 1);
        private readonly NoiseGenerator _generator;
        private readonly Splines _spline;
        private readonly int _yOffset;

        public SplinedNoiseGenerator(SplinedNoiseGeneratorData data, int yOffset)
        {
            _generator = new NoiseGenerator(data.noise, data.frequency);
            _spline = new Splines(data.spline);
            _yOffset = yOffset;
        }

        public float[] GenerateCache(Vector2Int pos, Vector2Int? size = null)
        {
            size ??= _size;
            pos = new Vector2Int(pos.x, _yOffset);

            var buffer = _generator.GenerateCache(pos, size.Value);
            for (var i = 0; i < buffer.Length; i++)
                buffer[i] = _spline.ApplySpline(buffer[i]);

            return buffer;
        }
    }
}