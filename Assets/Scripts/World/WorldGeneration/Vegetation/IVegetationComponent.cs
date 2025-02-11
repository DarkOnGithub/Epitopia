using UnityEngine;
using World.Blocks;
using World.Chunks;

namespace World.WorldGeneration.Vegetation
{
    public struct VegetationComponentData
    {
        public string Type { get; set; }
        public float[] Range { get; set; }
        public float Probability { get; set; }
        public string Block { get; set; }
    }
    public interface IVegetationComponent
    {
        public abstract bool CanGenerateAt(Chunk chunkIn, Vector2Int localPosition, Vector2Int origin, AbstractWorld world);
        public abstract void Generate(Chunk chunkIn, Vector2Int localPosition, Vector2Int origin, AbstractWorld world);
    }
}