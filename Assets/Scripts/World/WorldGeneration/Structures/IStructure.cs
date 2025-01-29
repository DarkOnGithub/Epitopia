using UnityEngine;

namespace World.WorldGeneration.Structures
{
    public struct StructureData
    {
        public int MaxPerChunk;
        public int MaxPerSuperChunk;
        public bool Detailed;
    }
    
    public interface IStructure
    {
        public Rect GetBounds();
    }
}