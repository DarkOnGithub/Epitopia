using System.Linq;
using UnityEngine;

namespace Tiles
{
    public class TreeTopTile : AutoTiles
    {
        public TreeTopTile(string path) : base($"{path}")
        {
            RegisterTileFromSprites(new()
                                    {
                                        null,null,null,
                                        null,null,null,
                                        null,null,null
                                    }, Sprites.ToList());
        }
    }
}