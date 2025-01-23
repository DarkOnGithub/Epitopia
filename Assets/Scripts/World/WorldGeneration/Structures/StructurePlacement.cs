using System.Collections.Generic;
using UnityEngine;
using World.Blocks;

namespace World.WorldGeneration.Structures
{
    public class StructurePlacement
    {
        public IStructure[] SurfaceStructures { get; }
        public IStructure[] VerticalStructures { get; }
        
        public StructurePlacement(IStructure[] surfaceStructures, IStructure[] verticalStructures)
        {
            SurfaceStructures = surfaceStructures;
            VerticalStructures = verticalStructures;
        }

        public List<Dictionary<Vector2Int, (IBlockState, bool)>> PlaceStructures(Vector2Int position, float point)
        {
            var result = new List<Dictionary<Vector2Int, (IBlockState, bool)>>();
            foreach (var structure in SurfaceStructures)
                result.Add(structure.TryPlace(position, point));
            return result;
        }


    }
}