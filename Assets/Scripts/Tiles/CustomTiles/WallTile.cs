using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class WallTile : AbstractTile
    {
        public WallTile(Texture2D texture) : base(texture)
        {
            if (texture == null) return;
            
            RegisterTileGroup(
                new[]
                {
                    (9, 3),
                    (10, 3),
                    (11, 3)
                }, new List<int?>
                   {
                       null, Empty, null,
                       Empty, null, Empty,
                       null, Empty, null
                   });
            
            RegisterTileGroup(
                new[]
                {
                    (1, 1),
                    (2, 1),
                    (3, 1)
                }, new List<int?>
                   {
                       null, Any, null,
                       Any, null, Any,
                       null, Any, null
                   });
            RegisterTileGroup(
                new[]
                {
                    (9, 0),
                    (9, 1),
                    (9, 2)
                }, new List<int?>
                   {
                       null, Empty, null,
                       Empty, null, Any,
                       null, Empty, null
                   });
            RegisterTileGroup(
                new[]
                {
                    (12, 0),
                    (12, 1),
                    (12, 2)
                }, new List<int?>
                   {
                       null, Empty, null,
                       Any, null, Empty,
                       null, Empty, null
                   });
            RegisterTileGroup(
                new[]
                {
                    (6, 0),
                    (7, 0),
                    (8, 0)
                }, new List<int?>
                   {
                       null, Empty, null,
                       Empty, null, Empty,
                       null, Any, null
                   });
            RegisterTileGroup(
                new[]
                {
                    (6, 3),
                    (7, 3),
                    (8, 3)
                }, new List<int?>
                   {
                       null, Any, null,
                       Empty, null, Empty,
                       null, Empty, null
                   });
            RegisterTileGroup(
                new[]
                {
                    (5, 0),
                    (5, 1),
                    (5, 2)
                }, new List<int?>
                   {
                       null, Any, null,
                       Empty, null, Empty,
                       null, Any, null
                   });
            RegisterTileGroup(
                new[]
                {
                    (6, 4),
                    (7, 4),
                    (8, 4)
                }, new List<int?>
                   {
                       null, Empty, null,
                       Any, null, Any,
                       null, Empty, null
                   });
            RegisterTileGroup(
                new[]
                {
                    (1, 3),
                    (3, 3),
                    (5, 3)
                }, new List<int?>
                   {
                       null, Empty, null,
                       Any, null, Empty,
                       null, Any, null
                   });
            RegisterTileGroup(
                new[]
                {
                    (0, 4),
                    (2, 4),
                    (4, 4)
                }, new List<int?>
                   {
                       null, Any, null,
                       Empty, null, Any,
                       null, Empty, null
                   });
            RegisterTileGroup(
                new[]
                {
                    (1, 4),
                    (3, 4),
                    (5, 4)
                }, new List<int?>
                   {
                       null, Any, null,
                       Any, null, Empty,
                       null, Empty, null
                   });
            RegisterTileGroup(
                new[]
                {
                    (0, 3),
                    (2, 3),
                    (4, 3)
                }, new List<int?>
                   {
                       null, Empty, null,
                       Empty, null, Any,
                       null, Any, null
                   });
            RegisterTileGroup(
                new[]
                {
                    (4, 0),
                    (4, 1),
                    (4, 2)
                }, new List<int?>
                   {
                       null, Any, null,
                       Any, null, Empty,
                       null, Empty, null
                   });
            RegisterTileGroup(
                new[]
                {
                    (1, 2),
                    (2, 2),
                    (3, 2)
                }, new List<int?>
                   {
                       null, Any, null,
                       Any, null, Any,
                       null, Empty, null
                   });

            RegisterTileGroup(
                new[]
                {
                    (0, 0),
                    (0, 1),
                    (0, 2)
                }, new List<int?>
                   {
                       null, Any, null,
                       Empty, null, Any,
                       null, Any, null
                   });
            RegisterTileGroup(
                new[]
                {
                    (4, 0),
                    (4, 1),
                    (4, 2)
                }, new List<int?>
                   {
                       null, Any, null,
                       Any, null, Empty,
                       null, Any, null
                   });
            RegisterTileGroup(
                new[]
                {
                    (1, 0),
                    (2, 0),
                    (3, 0)
                }, new List<int?>
                   {
                       null, Empty, null,
                       Any, null, Any,
                       null, Any, null
                   });
        }

        protected override Vector2Int Size { get; } = new(32, 32);
        protected override Vector2Int Padding { get; } = new(4, 4);
        protected override int PixelsPerUnit { get; } = 32;
    }
}