using System;
using UnityEngine;
using World.Blocks;

namespace World.WorldGeneration.Structures
{
    public struct FlowerVegetationData
    {
        public int[] Range;
        public float Probability;
        public string BlockName;
    }
    
    public class FlowerVegetation : IStructure
    {
        public IBlock[] Blocks;
        public float Probability { get; }
        public Func<float, bool> IsWithinRange;        
        public FlowerVegetation(FlowerVegetationData data)
        {
            var range = data.Range;
            if (range[0] == range[1])
                IsWithinRange = point => Mathf.Abs(point - range[0]) < 0.01f;
            else
                IsWithinRange = point => point >= range[0] && point <= range[1];
        }



        public Vector2 GetBounds()
        {
            throw new System.NotImplementedException();
        }

        public bool CanPlace(Vector2 pos, float point)
        {
            if(!IsWithinRange(point))
                return false;
            
        }

        public void Place(Vector2 pos)
        {
            throw new System.NotImplementedException();
        }
    }
}