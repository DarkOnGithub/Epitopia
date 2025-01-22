using UnityEngine;

namespace World.WorldGeneration.Noise
{
    public interface INoise
    {
        public float[] GenerateCache(Vector2Int pos, Vector2Int? size = null);
    }
}