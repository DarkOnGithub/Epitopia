using System.Linq;
using UnityEngine;

namespace Tiles
{
    public class FlowerTile : AutoTiles
    {
        public FlowerTile(string path) : base(path)
        {
            RegisterTileFromSprites(new()
                                    {
                                        null, null, null,
                                        null, null, null,
                                        null, null, null
                                    }, Sprites.ToList());            
        }
    }
}