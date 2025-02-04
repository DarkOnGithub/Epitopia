using UnityEngine;
using World.Chunks;

namespace World.Blocks.CustomBlocks
{
    public class DirtBlock : DefaultBlock
    {
        public DirtBlock(int id, string name, BlockProperties properties) : base(id, name, properties)
        {
            
        }

        public override void OnPlace(Chunk chunkIn, Vector2Int localPosition, IBlockState state)
        {
            Debug.Log($"Dirty block placed at {localPosition}");
        }
    }
}