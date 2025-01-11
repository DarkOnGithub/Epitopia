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

        public override bool RuleMatch(int neighbor, TileBase tile)
        {
            switch (neighbor)
            {
                case Neighbors.Any:
                    return tile is not null;
                case Neighbors.Empty:
                    return tile is null;
                case Neighbors.OnlyDirt:
                    return tile == DirtTile;
                case Neighbors.Different:
                    return tile != this && tile is not null;
                case TilingRuleOutput.Neighbor.This: return tile == this;
                case TilingRuleOutput.Neighbor.NotThis: return tile != this;
                case Neighbors.ThisOrWall:
                    return tile == this || (Walls.Contains(tile) && tile != null);
            }

            return base.RuleMatch(neighbor, tile);
        }

        public class Neighbors : TilingRuleOutput.Neighbor
        {
            public const int Any = 4;
            public const int Empty = 5;
            public const int OnlyDirt = 6;
            public const int Different = 7;
            public const int ThisOrWall = 8;
        }
    }
}