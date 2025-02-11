using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class TreeLogTile : AbstractTile
    {
        protected override Vector2Int Size { get; } = new Vector2Int(22, 22);
        protected override Vector2Int Padding { get; } = new Vector2Int(0, 0);

        public TreeLogTile(string spritePath) : base(GetTexture(spritePath))
        {
            
                     
                        RegisterTileGroup(new[]
                        {
                            (0, 0),
                (0, 1),
                (0, 2)
                        }, new List<int?>
                        {
                            null,
                1,
                null,
                2,
                null,
                2,
                null,
                2,
                null
                        });
                        RegisterTileGroup(new[]
                        {
                            (0, 3),
                (0, 4),
                (0, 5),
                (1, 0),
                (1, 1),
                (1, 2),
                (1, 3),
                (1, 4),
                (1, 5),
                (2, 3),
                (2, 4),
                (2, 5)
                        }, new List<int?>
                        {
                            null,
                1,
                null,
                2,
                null,
                2,
                null,
                1,
                null
                        });
                        RegisterTileGroup(new[]
                        {
                            (2, 0),
                (2, 1),
                (2, 2)
                        }, new List<int?>
                        {
                            null,
                1,
                null,
                1,
                null,
                1,
                null,
                1,
                null
                        });
                        RegisterTileGroup(new[]
                        {
                            (3, 3),
                (3, 4),
                (3, 5)
                        }, new List<int?>
                        {
                            null,
                1,
                null,
                6,
                null,
                4,
                null,
                1,
                null
                        });
                        RegisterTileGroup(new[]
                        {
                            (4, 0),
                (4, 1),
                (4, 2)
                        }, new List<int?>
                        {
                            null,
                1,
                null,
                4,
                null,
                6,
                null,
                1,
                null
                        });
                        RegisterTileGroup(new[]
                        {
                            (3, 6),
                (3, 7),
                (3, 8)
                        }, new List<int?>
                        {
                            null,
                1,
                null,
                1,
                null,
                2,
                null,
                6,
                null
                        });
                        RegisterTileGroup(new[]
                        {
                            (0, 6),
                (0, 7),
                (0, 8)
                        }, new List<int?>
                        {
                            null,
                1,
                null,
                2,
                null,
                1,
                null,
                6,
                null
                        });
                        RegisterTileGroup(new[]
                        {
                            (4, 6),
                (4, 7),
                (4, 8)
                        }, new List<int?>
                        {
                            null,
                1,
                null,
                1,
                null,
                1,
                null,
                6,
                null
                        });
                        RegisterTileGroup(new[]
                        {
                            (1, 6),
                (1, 7),
                (1, 8)
                        }, new List<int?>
                        {
                            null,
                null,
                null,
                1,
                null,
                null,
                AnySolid,
                AnySolid,
                null
                        });
                        RegisterTileGroup(new[]
                        {
                            (2, 6),
                (2, 7),
                (2, 8)
                        }, new List<int?>
                        {
                            null,
                null,
                null,
                null,
                null,
                1,
                null,
                AnySolid,
                AnySolid
                        });
                        RegisterTileGroup(new[]
                        {
                            (0, 9),
                (0, 10),
                (0, 11)
                        }, new List<int?>
                        {
                            null,
                6,
                null,
                null,
                null,
                null,
                null,
                null,
                null
                        });
            // var assetPath = $"Assets/GeneratedRuleTilelog.asset";
            // UnityEditor.AssetDatabase.CreateAsset(RuleTile, assetPath);
            // UnityEditor.AssetDatabase.SaveAssets();
        }
    }
}