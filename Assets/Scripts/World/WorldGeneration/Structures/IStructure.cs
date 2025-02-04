using UnityEngine;

namespace World.WorldGeneration.Structures
{
    public struct StructureData
    {
        public int MaxPerChunk;
        public int MaxPerSuperChunk;
        public float[] Range { get; set; }
        public float Probability { get; set; }
    }
    
    public interface IStructure
    {
        public StructureData Data { get; }
        public Rect GetBounds();
    }
}