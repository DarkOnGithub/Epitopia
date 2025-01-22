using UnityEngine;

namespace World.WorldGeneration.Structures
{
    public interface IStructure
    {
        public float Probability { get; }
        public Vector2 GetBounds();
        public bool CanPlace(Vector2 pos, float point);
        public void Place(Vector2 pos);
    }
}