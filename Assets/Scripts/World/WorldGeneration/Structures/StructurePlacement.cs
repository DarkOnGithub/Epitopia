using UnityEngine;

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

        public void PlaceStructures(Vector2Int position, float point)
        {
            foreach (var structure in SurfaceStructures)
            {
                if (structure.CanPlace(position, point))
                {
                    structure.Place(position);
                }
            }
        }
        
        
    }
}