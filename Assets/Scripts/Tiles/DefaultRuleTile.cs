using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tiles
{
    [CreateAssetMenu(fileName = "DefaultRuleTile", menuName = "2D/Tiles/DefaultRuleTile")]
    public class DefaultRuleTile : RuleTile<DefaultRuleTile.Neighbors>
    {
        public static RuleTile DirtTile;
        public static HashSet<TileBase> Walls = new();
        public static HashSet<TileBase> Solids = new();
        public override bool RuleMatch(int neighbor, TileBase tile)
        {
            switch (neighbor)
            {
                case Neighbors.Any:
                    return tile != null;
                case Neighbors.Empty:
                    return tile == null;
                case Neighbors.AnySolid:
                    return tile != null && Solids.Contains(tile);
                case Neighbors.AnySolidNotDirt:
                    return tile != null && tile != DirtTile;
                case Neighbors.EmptyOrNonSolid:
                    return tile == null || !Solids.Contains(tile);
                case Neighbors.Dirt:
                    return tile == DirtTile;
                case Neighbors.Wall:
                    return tile != null && Walls.Contains(tile);
                // Consider adding these additional neighbor types from commented code
                case Neighbors.Different:
                    return tile != null && tile != this;
                case Neighbors.ThisOrWall:
                    return tile == this || (tile != null && Walls.Contains(tile));

            }

            return base.RuleMatch(neighbor, tile);
        }

        public class Neighbors : TilingRuleOutput.Neighbor
        {
            public const int AnySolid = 4;
            public const int AnySolidNotDirt = 5;
            public const int EmptyOrNonSolid = 6;
            public const int Dirt = 7;
            public const int Wall = 8;
            public const int Any = 9;
            public const int Empty = 10;
            public const int Different = 11;
            public const int ThisOrWall = 12;
        }
    }
}