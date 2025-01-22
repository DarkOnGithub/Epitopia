using Newtonsoft.Json;
using UnityEngine;
using World.Chunks;

namespace World.WorldGeneration.Noise
{
    public struct ThresholdedNoiseGeneratorData
    {
        [JsonProperty] public string noise { get; set; }

        [JsonProperty] public float frequency { get; set; }

        [JsonProperty] public float threshold { get; set; }
    }

    public class ThresholdedNoiseGenerator
    {
        private static readonly Vector2Int _size = new(Chunk.ChunkSize, Chunk.ChunkSize);
        private readonly NoiseGenerator _generator;
        public ThresholdedNoiseGeneratorData Data;

        public ThresholdedNoiseGenerator(ThresholdedNoiseGeneratorData data)
        {
            Data = data;
            _generator = new NoiseGenerator(data.noise, data.frequency);
        }

        public float[] GenerateCache(Vector2Int pos, Vector2Int? size = null)
        {
            size ??= _size;
            return _generator.GenerateCache(pos, size.Value);
        }
    }
}