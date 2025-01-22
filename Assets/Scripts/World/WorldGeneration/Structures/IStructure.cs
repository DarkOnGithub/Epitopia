using UnityEngine;

namespace World.WorldGeneration.Structures
{
    public interface IStructure
    {
        public (int Min, int Max) Range { get; }
        public float Probability { get; }
        public Vector2 GetBounds();
        public void CanPlace(Vector2 pos);
        public void Place(Vector2 pos);
    }
}