using System.Linq;
using UnityEngine;

namespace Tiles
{
    public class TreeLeavesTile : AutoTiles
    {
        public TreeLeavesTile(string path) : base($"{path}")
        {
            var left = new Sprite[]
                       {
                           Sprites[0],
                           Sprites[2],
                           Sprites[4]
                       };
            var right = new Sprite[]
                        {
                            Sprites[1],
                            Sprites[3],
                            Sprites[5]
                        };
            RegisterTileFromSprites(new()
                                    {
                                        null, null, null,
                                        AnySolid, null,null,
                                        null,null,null
                                    }, right.ToList());
            RegisterTileFromSprites(new()
                                    {
                                        null, null, null,
                                        null, null, AnySolid,
                                        null, null, null
                                    }, left.ToList());
        }
    }
}