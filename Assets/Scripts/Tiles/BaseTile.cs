using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class BaseTile : AbstractTile
    {
        public static RuleTile DirtRuleTile;

        public BaseTile(Texture2D texture, bool mergeWithDirt, string name) : base(texture)
        {
            if (name == "Dirt")
            {
                DirtRuleTile = RuleTile;
                DefaultRuleTile.DirtTile = RuleTile;
            }

            RegisterTileGroup(new[]
                              {
                                  (0, 0),
                                  (0, 1),
                                  (0, 2)
                              }, new List<int?>
                                 {
                                     null, Any, null,
                                     Empty, null, null,
                                     null, Any, null
                                 });
            RegisterTileGroup(new[]
                              {
                                  (1, 0),
                                  (2, 0),
                                  (3, 0)
                              }, new List<int?>
                                 {
                                     null, Empty, null,
                                     Any, null, Any,
                                     null, null, null
                                 });
            //Aim Bot
            RegisterTileGroup(new[]
                              {
                                  (1, 2),
                                  (2, 2),
                                  (3, 2)
                              }, new List<int?>
                                 {
                                     null, null, null,
                                     Any, null, Any,
                                     null, Empty, null
                                 });
            //Aim Right
            RegisterTileGroup(new[]
                              {
                                  (4, 0),
                                  (5, 1),
                                  (6, 2)
                              }, new List<int?>
                                 {
                                     null, Any, null,
                                     null, null, Empty,
                                     null, Any, null
                                 });
            RegisterTileGroup(new[]
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
            RegisterTileGroup(new[]
                              {
                                  (4, 0),
                                  (5, 1),
                                  (6, 2)
                              }, new List<int?>
                                 {
                                     null, Any, null,
                                     null, null, Empty,
                                     null, Any, null
                                 });
            RegisterTileGroup(new[]
                              {
                                  (6, 0),
                                  (6, 1),
                                  (6, 2)
                              }, new List<int?>
                                 {
                                     null, This, null,
                                     Empty, null, Empty,
                                     null, This, null
                                 }, RuleTile.TilingRuleOutput.Transform.MirrorY);

            RegisterTileGroup(new[]
                              {
                                  (6, 0),
                                  (7, 0),
                                  (8, 0)
                              }, new List<int?>
                                 {
                                     null, Empty, null,
                                     Empty, null, Empty,
                                     null, This, null
                                 });

            RegisterTileGroup(new[]
                              {
                                  (6, 3),
                                  (7, 3),
                                  (8, 3)
                              }, new List<int?>
                                 {
                                     null, This, null,
                                     Empty, null, Empty,
                                     null, Empty, null
                                 });


            RegisterTileGroup(new[]
                              {
                                  (6, 4),
                                  (7, 4),
                                  (8, 4)
                              }, new List<int?>
                                 {
                                     null, Empty, null,
                                     This, null, This,
                                     null, Empty, null
                                 }, RuleTile.TilingRuleOutput.Transform.MirrorX);

            RegisterTileGroup(new[]
                              {
                                  (9, 0),
                                  (9, 1),
                                  (9, 2)
                              }, new List<int?>
                                 {
                                     null, Empty, null,
                                     Empty, null, This,
                                     null, Empty, null
                                 });

            RegisterTileGroup(new[]
                              {
                                  (12, 0),
                                  (12, 1),
                                  (12, 2)
                              }, new List<int?>
                                 {
                                     null, Empty, null,
                                     This, null, Empty,
                                     null, Empty, null
                                 });

            RegisterTileGroup(new[]
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

            RegisterTileGroup(new[]
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

            RegisterTileGroup(new[]
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

            RegisterTileGroup(new[]
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

            RegisterTileGroup(new[]
                              {
                                  (1, 4),
                                  (3, 4),
                                  (4, 4)
                              }, new List<int?>
                                 {
                                     null, Any, null,
                                     Any, null, Empty,
                                     null, Empty, null
                                 });
            if (!mergeWithDirt)
                return;

            RegisterTileGroup(new[]
                              {
                                  (2, 5),
                                  (2, 7),
                                  (2, 9)
                              }, new List<int?>
                                 {
                                     null, Any, null,
                                     Any, null, Any,
                                     null, Any, Empty
                                 });
            RegisterTileGroup(new[]
                              {
                                  (2, 6),
                                  (2, 8),
                                  (2, 10)
                              }, new List<int?>
                                 {
                                     null, Any, Empty,
                                     Any, null, Any,
                                     null, Any, null
                                 });
            RegisterTileGroup(new[]
                              {
                                  (3, 5),
                                  (3, 7),
                                  (3, 9)
                              }, new List<int?>
                                 {
                                     null, Any, null,
                                     Any, null, Any,
                                     Empty, Any, null
                                 });
            RegisterTileGroup(new[]
                              {
                                  (3, 6),
                                  (3, 8),
                                  (3, 10)
                              }, new List<int?>
                                 {
                                     Empty, Any, null,
                                     Any, null, Any,
                                     null, Any, null
                                 });
        }
    }
}