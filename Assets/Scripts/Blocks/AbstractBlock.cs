using UnityEngine;
using UnityEngine.Tilemaps;

namespace Blocks
{
    public abstract class AbstractBlock
    {
        private static int _blockCounter = 0;
        public int Id;
        public string Name;
        public BlockProperties Properties;
        public readonly Tile Tile;
        public AbstractBlock(string name, BlockProperties properties)
        {
            Id = _blockCounter++;
            Name = name;
            Properties = properties;
            Tile = properties.GetTile();
        }
    }
}