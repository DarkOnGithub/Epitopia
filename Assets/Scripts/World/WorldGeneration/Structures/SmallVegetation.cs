using UnityEngine;
using World.Blocks;

namespace World.WorldGeneration.Structures
{
    public class SmallVegetation : IStructure
    {
        public IBlock[] Blocks;
        public (int Min, int Max) Range { get; }
        public float Probability { get; }
        
        public SmallVegetation(IBlock[] blocks)
        {
            Blocks = blocks;
        }



        public Vector2 GetBounds()
        {
            throw new System.NotImplementedException();
        }

        public void CanPlace(Vector2 pos)
        {
            throw new System.NotImplementedException();
        }

        public void Place(Vector2 pos)
        {
            throw new System.NotImplementedException();
        }
    }
}