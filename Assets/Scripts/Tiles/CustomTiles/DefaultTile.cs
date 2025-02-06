using System.Collections.Generic;
using UnityEngine;

namespace Tiles
{
    public class DefaultTile : AbstractTile
    {

        public DefaultTile(string path, bool mergeWithDirt = false) : base(GetTexture(path))
        {
            // RegisterTileGroup(new[]
            //                   {
            //                       (1, 0),
            //                       (2, 0),
            //                       (3, 0)
            //                   }, new List<int?>
            //                      {
            //                      });
            // RegisterTileGroup(new[]
            // {
            //     (0, 0),
            //     (0, 1),
            //     (0, 2)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (1, 0),
            //     (2, 0),
            //     (3, 0)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (1, 1),
            //     (2, 1),
            //     (3, 1)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (1, 2),
            //     (2, 2),
            //     (3, 2)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (4, 0),
            //     (4, 1),
            //     (4, 2)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (5, 0),
            //     (5, 1),
            //     (5, 2)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (6, 0),
            //     (7, 0),
            //     (8, 0)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (6, 1),
            //     (7, 1),
            //     (8, 1)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (6, 2),
            //     (7, 2),
            //     (8, 2)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (6, 3),
            //     (7, 3),
            //     (8, 3)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (9, 0),
            //     (9, 1),
            //     (9, 2)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (10, 3),
            //     (11, 3),
            //     (12, 3)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (6, 4),
            //     (7, 4),
            //     (8, 4)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (10, 0),
            //     (10, 1),
            //     (10, 2)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (11, 0),
            //     (11, 1),
            //     (11, 2)
            // }, new List<int?> { });
            //
            // RegisterTileGroup(new[]
            // {
            //     (12, 0),
            //     (12, 1),
            //     (12, 2)
            // }, new List<int?> { });
            //
            // RegisterTileGroup(new[]
            // {
            //     (0, 3),
            //     (2, 3),
            //     (4, 3)
            // }, new List<int?> { });
            //
            // RegisterTileGroup(new[]
            // {
            //     (1, 3),
            //     (3, 3),
            //     (5, 3)
            // }, new List<int?> { });
            //
            // RegisterTileGroup(new[]
            // {
            //     (0, 4),
            //     (2, 4),
            //     (4, 4)
            // }, new List<int?> { });
            //
            // RegisterTileGroup(new[]
            // {
            //     (1, 4),
            //     (3, 4),
            //     (5, 4)
            // }, new List<int?> { });
            //
            //
            // if (!mergeWithDirt) return;
            //
            //
            // RegisterTileGroup(new[]
            // {
            //     (13, 0),
            //     (14, 0),
            //     (15, 0)
            // }, new List<int?> { });
            //
            // RegisterTileGroup(new[]
            // {
            //     (13, 1),
            //     (14, 1),
            //     (15, 1)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (13, 2),
            //     (14, 2),
            //     (15, 2)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (13, 3),
            //     (14, 3),
            //     (15, 3)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (0, 5),
            //     (0, 7),
            //     (0, 9)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (0, 6),
            //     (0, 8),
            //     (0, 10)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (1, 5),
            //     (1, 7),
            //     (1, 9)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (1, 6),
            //     (1, 8),
            //     (1, 10)
            // }, new List<int?> { });
            //
            //
            // RegisterTileGroup(new[]
            // {
            //     (2, 5),
            //     (2, 7),
            //     (2, 9)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (2, 6),
            //     (2, 8),
            //     (2, 10)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (3, 5),
            //     (3, 7),
            //     (3, 9)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (3, 6),
            //     (3, 8),
            //     (3, 10)
            // }, new List<int?> { });
            //
            // RegisterTileGroup(new[]
            // {
            //     (4, 5),
            //     (4, 6),
            //     (4, 7)
            // }, new List<int?> { });
            //
            // RegisterTileGroup(new[]
            // {
            //     (4, 8),
            //     (4, 9),
            //     (4, 10)
            // }, new List<int?> { });
            //
            // RegisterTileGroup(new[]
            // {
            //     (5, 5),
            //     (5, 6),
            //     (5, 7)
            // }, new List<int?> { });
            //
            // RegisterTileGroup(new[]
            // {
            //     (5, 8),
            //     (5, 9),
            //     (5, 10)
            // }, new List<int?> { });
            //
            //
            // RegisterTileGroup(new[]
            // {
            //     (6, 5),
            //     (6, 6),
            //     (6, 7)
            // }, new List<int?> { });
            //
            // RegisterTileGroup(new[]
            // {
            //     (6, 8),
            //     (6, 9),
            //     (6, 10)
            // }, new List<int?> { });
            //
            // RegisterTileGroup(new[]
            // {
            //     (7, 5),
            //     (7, 6),
            //     (7, 7)
            // }, new List<int?> { });
            //
            // RegisterTileGroup(new[]
            // {
            //     (7, 8),
            //     (7, 9),
            //     (7, 10)
            // }, new List<int?> { });
            //
            //
            // RegisterTileGroup(new[]
            // {
            //     (8, 5),
            //     (9, 5),
            //     (10, 5)
            // }, new List<int?> { });
            //
            // RegisterTileGroup(new[]
            // {
            //     (8, 6),
            //     (9, 6),
            //     (10, 6)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (8, 10),
            //     (9, 10),
            //     (10, 10)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (8, 7),
            //     (8, 8),
            //     (8, 9)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (9, 7),
            //     (9, 8),
            //     (9, 9)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (10, 7),
            //     (10, 8),
            //     (10, 9)
            // }, new List<int?> { });
            //
            // RegisterTileGroup(new[]
            // {
            //     (11, 5),
            //     (11, 6),
            //     (11, 7)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (12, 5),
            //     (12, 6),
            //     (12, 7)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (11, 8),
            //     (11, 9),
            //     (11, 10)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (12, 8),
            //     (12, 9),
            //     (12, 10)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (0, 11),
            //     (1, 11),
            //     (2, 11)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (0, 12),
            //     (1, 12),
            //     (2, 12)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (3, 11),
            //     (4, 11),
            //     (5, 11)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (3, 12),
            //     (4, 12),
            //     (5, 12)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (6, 11),
            //     (7, 11),
            //     (8, 11)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (9, 11),
            //     (10, 11),
            //     (11, 11)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (0, 13),
            //     (1, 13),
            //     (2, 13)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (3, 13),
            //     (4, 13),
            //     (5, 13)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (0, 14),
            //     (1, 14),
            //     (2, 14)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (3, 14),
            //     (4, 14),
            //     (5, 14)
            // }, new List<int?> { });
            // RegisterTileGroup(new[]
            // {
            //     (6, 12),
            //     (6, 13),
            //     (6, 14)
            // }, new List<int?> { });

            if (mergeWithDirt)
            {
                RegisterTileGroup(new[]
                                  {
                                      (13, 0),
                                      (14, 0),
                                      (15, 0)
                                  }, new List<int?>
                                     {
                                         null,
                                         6,
                                         null,
                                         4,
                                         null,
                                         4,
                                         null,
                                         7,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (13, 1),
                                      (14, 1),
                                      (15, 1)
                                  }, new List<int?>
                                     {
                                         null,
                                         7,
                                         null,
                                         4,
                                         null,
                                         4,
                                         null,
                                         6,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (13, 2),
                                      (14, 2),
                                      (15, 2)
                                  }, new List<int?>
                                     {
                                         null,
                                         4,
                                         null,
                                         6,
                                         null,
                                         7,
                                         null,
                                         4,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (13, 3),
                                      (14, 3),
                                      (15, 3)
                                  }, new List<int?>
                                     {
                                         null,
                                         4,
                                         null,
                                         7,
                                         null,
                                         6,
                                         null,
                                         4,
                                         null
                                     });


        

                RegisterTileGroup(new[]
                                  {
                                      (4, 5),
                                      (4, 6),
                                      (4, 7)
                                  }, new List<int?>
                                     {
                                         null,
                                         4,
                                         null,
                                         6,
                                         null,
                                         4,
                                         null,
                                         7,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (4, 8),
                                      (4, 9),
                                      (4, 10)
                                  }, new List<int?>
                                     {
                                         null,
                                         7,
                                         null,
                                         6,
                                         null,
                                         4,
                                         null,
                                         4,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (5, 5),
                                      (5, 6),
                                      (5, 7)
                                  }, new List<int?>
                                     {
                                         null,
                                         4,
                                         null,
                                         4,
                                         null,
                                         6,
                                         null,
                                         7,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (5, 8),
                                      (5, 9),
                                      (5, 10)
                                  }, new List<int?>
                                     {
                                         null,
                                         7,
                                         null,
                                         4,
                                         null,
                                         6,
                                         null,
                                         4,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (6, 5),
                                      (6, 6),
                                      (6, 7)
                                  }, new List<int?>
                                     {
                                         null,
                                         6,
                                         null,
                                         6,
                                         null,
                                         6,
                                         null,
                                         7,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (6, 8),
                                      (6, 9),
                                      (6, 10)
                                  }, new List<int?>
                                     {
                                         null,
                                         7,
                                         null,
                                         6,
                                         null,
                                         6,
                                         null,
                                         6,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (7, 5),
                                      (7, 6),
                                      (7, 7)
                                  }, new List<int?>
                                     {
                                         null,
                                         4,
                                         null,
                                         6,
                                         null,
                                         6,
                                         null,
                                         7,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (7, 8),
                                      (7, 9),
                                      (7, 10)
                                  }, new List<int?>
                                     {
                                         null,
                                         7,
                                         null,
                                         6,
                                         null,
                                         6,
                                         null,
                                         4,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (8, 5),
                                      (9, 5),
                                      (10, 5)
                                  }, new List<int?>
                                     {
                                         null,
                                         4,
                                         null,
                                         4,
                                         null,
                                         4,
                                         null,
                                         7,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (8, 6),
                                      (9, 6),
                                      (10, 6)
                                  }, new List<int?>
                                     {
                                         null,
                                         7,
                                         null,
                                         4,
                                         null,
                                         4,
                                         null,
                                         4,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (8, 10),
                                      (9, 10),
                                      (10, 10)
                                  }, new List<int?>
                                     {
                                         null,
                                         7,
                                         null,
                                         4,
                                         null,
                                         4,
                                         null,
                                         7,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (8, 7),
                                      (8, 8),
                                      (8, 9)
                                  }, new List<int?>
                                     {
                                         null,
                                         4,
                                         null,
                                         4,
                                         null,
                                         7,
                                         null,
                                         4,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (9, 7),
                                      (9, 8),
                                      (9, 9)
                                  }, new List<int?>
                                     {
                                         null,
                                         4,
                                         null,
                                         7,
                                         null,
                                         4,
                                         null,
                                         4,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (10, 7),
                                      (10, 8),
                                      (10, 9)
                                  }, new List<int?>
                                     {
                                         null,
                                         4,
                                         null,
                                         7,
                                         null,
                                         7,
                                         null,
                                         4,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (11, 5),
                                      (11, 6),
                                      (11, 7)
                                  }, new List<int?>
                                     {
                                         null,
                                         7,
                                         null,
                                         7,
                                         null,
                                         7,
                                         null,
                                         4,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (12, 5),
                                      (12, 6),
                                      (12, 7)
                                  }, new List<int?>
                                     {
                                         null,
                                         7,
                                         null,
                                         7,
                                         null,
                                         4,
                                         null,
                                         7,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (11, 8),
                                      (11, 9),
                                      (11, 10)
                                  }, new List<int?>
                                     {
                                         null,
                                         4,
                                         null,
                                         7,
                                         null,
                                         7,
                                         null,
                                         7,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (12, 8),
                                      (12, 9),
                                      (12, 10)
                                  }, new List<int?>
                                     {
                                         null,
                                         7,
                                         null,
                                         4,
                                         null,
                                         7,
                                         null,
                                         7,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (0, 11),
                                      (1, 11),
                                      (2, 11)
                                  }, new List<int?>
                                     {
                                         null,
                                         6,
                                         null,
                                         7,
                                         null,
                                         4,
                                         null,
                                         4,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (0, 12),
                                      (1, 12),
                                      (2, 12)
                                  }, new List<int?>
                                     {
                                         null,
                                         4,
                                         null,
                                         7,
                                         null,
                                         4,
                                         null,
                                         6,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (3, 11),
                                      (4, 11),
                                      (5, 11)
                                  }, new List<int?>
                                     {
                                         null,
                                         6,
                                         null,
                                         4,
                                         null,
                                         7,
                                         null,
                                         4,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (3, 12),
                                      (4, 12),
                                      (5, 12)
                                  }, new List<int?>
                                     {
                                         null,
                                         4,
                                         null,
                                         4,
                                         null,
                                         7,
                                         null,
                                         6,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (6, 11),
                                      (7, 11),
                                      (8, 11)
                                  }, new List<int?>
                                     {
                                         null,
                                         7,
                                         null,
                                         7,
                                         null,
                                         7,
                                         null,
                                         7,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (9, 11),
                                      (10, 11),
                                      (11, 11)
                                  }, new List<int?>
                                     {
                                         null,
                                         6,
                                         null,
                                         7,
                                         null,
                                         7,
                                         null,
                                         6,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (0, 13),
                                      (1, 13),
                                      (2, 13)
                                  }, new List<int?>
                                     {
                                         null,
                                         6,
                                         null,
                                         7,
                                         null,
                                         6,
                                         null,
                                         6,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (3, 13),
                                      (4, 13),
                                      (5, 13)
                                  }, new List<int?>
                                     {
                                         null,
                                         6,
                                         null,
                                         6,
                                         null,
                                         7,
                                         null,
                                         6,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (0, 14),
                                      (1, 14),
                                      (2, 14)
                                  }, new List<int?>
                                     {
                                         null,
                                         6,
                                         null,
                                         7,
                                         null,
                                         4,
                                         null,
                                         6,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (3, 14),
                                      (4, 14),
                                      (5, 14)
                                  }, new List<int?>
                                     {
                                         null,
                                         6,
                                         null,
                                         4,
                                         null,
                                         7,
                                         null,
                                         6,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (6, 12),
                                      (6, 13),
                                      (6, 14)
                                  }, new List<int?>
                                     {
                                         null,
                                         7,
                                         null,
                                         6,
                                         null,
                                         6,
                                         null,
                                         7,
                                         null
                                     });
                RegisterTileGroup(new[]
                                  {
                                      (0, 5),
                                      (0, 7),
                                      (0, 9)
                                  }, new List<int?>
                                     {
                                         null,
                                         4,
                                         null,
                                         4,
                                         null,
                                         4,
                                         null,
                                         4,
                                         7
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (0, 6),
                                      (0, 8),
                                      (0, 10)
                                  }, new List<int?>
                                     {
                                         null,
                                         4,
                                         7,
                                         4,
                                         null,
                                         4,
                                         null,
                                         4,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (1, 5),
                                      (1, 7),
                                      (1, 9)
                                  }, new List<int?>
                                     {
                                         null,
                                         4,
                                         null,
                                         4,
                                         null,
                                         4,
                                         7,
                                         4,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (1, 6),
                                      (1, 8),
                                      (1, 10)
                                  }, new List<int?>
                                     {
                                         7,
                                         4,
                                         null,
                                         4,
                                         null,
                                         4,
                                         null,
                                         4,
                                         null
                                     });
                        RegisterTileGroup(new[]
                                  {
                                      (2, 5),
                                      (2, 7),
                                      (2, 9)
                                  }, new List<int?>
                                     {
                                         null,
                                         7,
                                         null,
                                         7,
                                         null,
                                         4,
                                         null,
                                         4,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (2, 6),
                                      (2, 8),
                                      (2, 10)
                                  }, new List<int?>
                                     {
                                         null,
                                         4,
                                         null,
                                         7,
                                         null,
                                         4,
                                         null,
                                         7,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (3, 5),
                                      (3, 7),
                                      (3, 9)
                                  }, new List<int?>
                                     {
                                         null,
                                         7,
                                         null,
                                         4,
                                         null,
                                         7,
                                         null,
                                         4,
                                         null
                                     });

                RegisterTileGroup(new[]
                                  {
                                      (3, 6),
                                      (3, 8),
                                      (3, 10)
                                  }, new List<int?>
                                     {
                                         null,
                                         4,
                                         null,
                                         4,
                                         null,
                                         7,
                                         null,
                                         7,
                                         null
                                     });
            }

            RegisterTileGroup(new[]
                              {
                                  (0, 0),
                                  (0, 1),
                                  (0, 2)
                              }, new List<int?>
                                 {
                                     null,
                                     4,
                                     null,
                                     6,
                                     null,
                                     4,
                                     null,
                                     4,
                                     null
                                 });

            RegisterTileGroup(new[]
                              {
                                  (1, 0),
                                  (2, 0),
                                  (3, 0)
                              }, new List<int?>
                                 {
                                     null,
                                     6,
                                     null,
                                     4,
                                     null,
                                     4,
                                     null,
                                     4,
                                     null
                                 });


            RegisterTileGroup(new[]
                              {
                                  (1, 2),
                                  (2, 2),
                                  (3, 2)
                              }, new List<int?>
                                 {
                                     null,
                                     4,
                                     null,
                                     4,
                                     null,
                                     4,
                                     null,
                                     6,
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
                                     4,
                                     null,
                                     4,
                                     null,
                                     6,
                                     null,
                                     4,
                                     null
                                 });

            RegisterTileGroup(new[]
                              {
                                  (5, 0),
                                  (5, 1),
                                  (5, 2)
                              }, new List<int?>
                                 {
                                     null,
                                     4,
                                     null,
                                     6,
                                     null,
                                     6,
                                     null,
                                     4,
                                     null
                                 });

            RegisterTileGroup(new[]
                              {
                                  (6, 0),
                                  (7, 0),
                                  (8, 0)
                              }, new List<int?>
                                 {
                                     null,
                                     6,
                                     null,
                                     6,
                                     null,
                                     6,
                                     null,
                                     4,
                                     null
                                 });

            RegisterTileGroup(new[]
                              {
                                  (6, 1),
                                  (7, 1),
                                  (8, 1)
                              }, new List<int?>
                                 {
                                     12,
                                     12,
                                     12,
                                     12,
                                     null,
                                     12,
                                     12,
                                     12,
                                     12
                                 });

            RegisterTileGroup(new[]
                              {
                                  (6, 2),
                                  (7, 2),
                                  (8, 2)
                              }, new List<int?>
                                 {
                                     12,
                                     12,
                                     12,
                                     12,
                                     null,
                                     12,
                                     12,
                                     12,
                                     12
                                 });

            RegisterTileGroup(new[]
                              {
                                  (6, 3),
                                  (7, 3),
                                  (8, 3)
                              }, new List<int?>
                                 {
                                     null,
                                     4,
                                     null,
                                     6,
                                     null,
                                     6,
                                     null,
                                     6,
                                     null
                                 });

            RegisterTileGroup(new[]
                              {
                                  (9, 0),
                                  (9, 1),
                                  (9, 2)
                              }, new List<int?>
                                 {
                                     null,
                                     6,
                                     null,
                                     6,
                                     null,
                                     4,
                                     null,
                                     6,
                                     null
                                 });

            RegisterTileGroup(new[]
                              {
                                  (10, 3),
                                  (11, 3),
                                  (12, 3)
                              }, new List<int?>
                                 {
                                     null,
                                     6,
                                     null,
                                     6,
                                     null,
                                     6,
                                     null,
                                     6,
                                     null
                                 });

            RegisterTileGroup(new[]
                              {
                                  (6, 4),
                                  (7, 4),
                                  (8, 4)
                              }, new List<int?>
                                 {
                                     null,
                                     6,
                                     null,
                                     4,
                                     null,
                                     4,
                                     null,
                                     6,
                                     null
                                 });

            RegisterTileGroup(new[]
                              {
                                  (10, 0),
                                  (10, 1),
                                  (10, 2)
                              }, new List<int?>
                                 {
                                     12,
                                     12,
                                     12,
                                     12,
                                     null,
                                     12,
                                     12,
                                     12,
                                     12
                                 });

            RegisterTileGroup(new[]
                              {
                                  (11, 0),
                                  (11, 1),
                                  (11, 2)
                              }, new List<int?>
                                 {
                                     12,
                                     12,
                                     12,
                                     12,
                                     null,
                                     12,
                                     12,
                                     12,
                                     12
                                 });

            RegisterTileGroup(new[]
                              {
                                  (12, 0),
                                  (12, 1),
                                  (12, 2)
                              }, new List<int?>
                                 {
                                     null,
                                     6,
                                     null,
                                     4,
                                     null,
                                     6,
                                     null,
                                     6,
                                     null
                                 });

            RegisterTileGroup(new[]
                              {
                                  (0, 3),
                                  (2, 3),
                                  (4, 3)
                              }, new List<int?>
                                 {
                                     null,
                                     6,
                                     null,
                                     6,
                                     null,
                                     4,
                                     null,
                                     4,
                                     null
                                 });

            RegisterTileGroup(new[]
                              {
                                  (1, 3),
                                  (3, 3),
                                  (5, 3)
                              }, new List<int?>
                                 {
                                     null,
                                     6,
                                     null,
                                     4,
                                     null,
                                     6,
                                     null,
                                     4,
                                     null
                                 });

            RegisterTileGroup(new[]
                              {
                                  (0, 4),
                                  (2, 4),
                                  (4, 4)
                              }, new List<int?>
                                 {
                                     null,
                                     4,
                                     null,
                                     6,
                                     null,
                                     4,
                                     null,
                                     6,
                                     null
                                 });

            RegisterTileGroup(new[]
                              {
                                  (1, 4),
                                  (3, 4),
                                  (5, 4)
                              }, new List<int?>
                                 {
                                     null,
                                     4,
                                     null,
                                     4,
                                     null,
                                     6,
                                     null,
                                     6,
                                     null
                                 });

            RegisterTileGroup(new[]
                              {
                                  (1, 1),
                                  (2, 1),
                                  (3, 1)
                              }, new List<int?>
                                 {
                                     null,
                                     4,
                                     null,
                                     4,
                                     null,
                                     4,
                                     null,
                                     4,
                                     null
                                 });
            // if (mergeWithDirt)
            // {
            //      RegisterTileGroup(new[]
            //                   {
            //                       (0, 5),
            //                       (0, 7),
            //                       (0, 9)
            //                   }, new List<int?>
            //                      {
            //                          null, null, null,
            //                          null, null, This,
            //                          null, This, Dirt
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (0, 6),
            //                       (0, 8),
            //                       (0, 10)
            //                   }, new List<int?>
            //                      {
            //                          null, This, Dirt,
            //                          null, null, This,
            //                          null, null, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (1, 5),
            //                       (1, 7),
            //                       (1, 9)
            //                   }, new List<int?>
            //                      {
            //                          null, null, null,
            //                          This, null, null,
            //                          Dirt, This, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (1, 6),
            //                       (1, 8),
            //                       (1, 10)
            //                   }, new List<int?>
            //                      {
            //                          Dirt, This, null,
            //                          This, null, null,
            //                          null, null, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (13, 0),
            //                       (14, 0),
            //                       (15, 0)
            //                   }, new List<int?>
            //                      {
            //                          null, null, null,
            //                          This, null, This,
            //                          null, Dirt, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (13, 1),
            //                       (14, 1),
            //                       (15, 1)
            //                   }, new List<int?>
            //                      {
            //                          null, Dirt, null,
            //                          This, null, This,
            //
            //                          null, null, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (13, 2),
            //                       (14, 2),
            //                       (15, 2)
            //                   }, new List<int?>
            //                      {
            //                          null, This, null,
            //                          null, null, Dirt,
            //                          null, This, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (13, 3),
            //                       (14, 3),
            //                       (15, 3)
            //                   }, new List<int?>
            //                      {
            //                          null, This, null,
            //                          Dirt, null, null,
            //                          null, This, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (2, 5),
            //                       (2, 7),
            //                       (2, 9)
            //                   }, new List<int?>
            //                      {
            //                          null, Dirt, null,
            //                          Dirt, null, This,
            //                          null, This, EmptyOrNonSolid
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (3, 5),
            //                       (3, 7),
            //                       (3, 9)
            //                   }, new List<int?>
            //                      {
            //                          null, Dirt, null,
            //                          This, null, Dirt,
            //                          EmptyOrNonSolid, This, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (2, 6),
            //                       (2, 8),
            //                       (2, 10)
            //                   }, new List<int?>
            //                      {
            //                          null, This, EmptyOrNonSolid,
            //                          Dirt, null, This,
            //                          null, Dirt, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (3, 6),
            //                       (3, 8),
            //                       (3, 10)
            //                   }, new List<int?>
            //                      {
            //                          EmptyOrNonSolid, This, null,
            //                          This, null, Dirt,
            //                          null, Dirt, null
            //                      });
            //
            // RegisterTileGroup(new[]
            //                   {
            //                       (4, 5),
            //                       (4, 6),
            //                       (4, 7)
            //                   }, new List<int?>
            //                      {
            //                          EmptyOrNonSolid, This, null,
            //                          EmptyOrNonSolid, null, Dirt,
            //                          Dirt, Dirt, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (4, 8),
            //                       (4, 9),
            //                       (4, 10)
            //                   }, new List<int?>
            //                      {
            //                          Dirt, Dirt, null,
            //                          EmptyOrNonSolid, null, Dirt,
            //                          EmptyOrNonSolid, This, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (5, 5),
            //                       (5, 6),
            //                       (5, 7)
            //                   }, new List<int?>
            //                      {
            //                          null, This, EmptyOrNonSolid,
            //                          Dirt, null, EmptyOrNonSolid,
            //                          null, Dirt, Dirt
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (5, 8),
            //                       (5, 9),
            //                       (5, 10)
            //                   }, new List<int?>
            //                      {
            //                          null, This, EmptyOrNonSolid,
            //                          Dirt, null, EmptyOrNonSolid,
            //                          null, Dirt, Dirt
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (6, 5),
            //                       (6, 6),
            //                       (6, 7)
            //                   }, new List<int?>
            //                      {
            //                          null, EmptyOrNonSolid, null,
            //                          EmptyOrNonSolid, null, EmptyOrNonSolid,
            //                          null, Dirt, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (6, 8),
            //                       (6, 9),
            //                       (6, 10)
            //                   }, new List<int?>
            //                      {
            //                          null, Dirt, null,
            //                          EmptyOrNonSolid, null, EmptyOrNonSolid,
            //                          null, EmptyOrNonSolid, null
            //                      });
            //
            // RegisterTileGroup(new[]
            //                   {
            //                       (7, 5),
            //                       (7, 6),
            //                       (7, 7)
            //                   }, new List<int?>
            //                      {
            //                          null, This, null,
            //                          EmptyOrNonSolid, null, EmptyOrNonSolid,
            //                          null, Dirt, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (7, 8),
            //                       (7, 9),
            //                       (7, 10)
            //                   }, new List<int?>
            //                      {
            //                          null, Dirt, null,
            //                          EmptyOrNonSolid, null, EmptyOrNonSolid,
            //                          null, This, null
            //                      });
            //
            // RegisterTileGroup(new[]
            //                   {
            //                       (10, 7),
            //                       (10, 8),
            //                       (10, 9)
            //                   }, new List<int?>
            //                      {
            //                          null, This, null,
            //                          Dirt, null, Dirt,
            //                          null, This, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (8, 10),
            //                       (9, 10),
            //                       (10, 10)
            //                   }, new List<int?>
            //                      {
            //                          null, Dirt, null,
            //                          This, null, This,
            //                          null, Dirt, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (0, 11),
            //                       (1, 11),
            //                       (2, 11)
            //                   }, new List<int?>
            //                      {
            //                          Dirt, EmptyOrNonSolid, EmptyOrNonSolid,
            //                          Dirt, null, This,
            //                          null, Dirt, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (3, 11),
            //                       (4, 11),
            //                       (5, 11)
            //                   }, new List<int?>
            //                      {
            //                          EmptyOrNonSolid, EmptyOrNonSolid, Dirt,
            //                          This, null, Dirt,
            //                          null, Dirt, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (0, 12),
            //                       (1, 12),
            //                       (2, 12)
            //                   }, new List<int?>
            //                      {
            //                          null, Dirt, null,
            //                          Dirt, null, This,
            //                          Dirt, EmptyOrNonSolid, EmptyOrNonSolid
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (3, 12),
            //                       (4, 12),
            //                       (5, 12)
            //                   }, new List<int?>
            //                      {
            //                          null, Dirt, null,
            //                          This, null, Dirt,
            //                          EmptyOrNonSolid, EmptyOrNonSolid, Dirt
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (0, 13),
            //                       (1, 13),
            //                       (2, 13)
            //                   }, new List<int?>
            //                      {
            //                          null, EmptyOrNonSolid, null,
            //                          Dirt, null, EmptyOrNonSolid,
            //                          null, EmptyOrNonSolid, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (3, 13),
            //                       (4, 13),
            //                       (5, 13)
            //                   }, new List<int?>
            //                      {
            //                          null, EmptyOrNonSolid, null,
            //                          EmptyOrNonSolid, null, Dirt,
            //                          null, EmptyOrNonSolid, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (0, 14),
            //                       (1, 14),
            //                       (2, 14)
            //                   }, new List<int?>
            //                      {
            //                          null, EmptyOrNonSolid, null,
            //                          Dirt, null, This,
            //                          null, EmptyOrNonSolid, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (3, 14),
            //                       (4, 14),
            //                       (5, 14)
            //                   }, new List<int?>
            //                      {
            //                          null, EmptyOrNonSolid, null,
            //                          This, null, Dirt,
            //                          null, EmptyOrNonSolid, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (0, 15),
            //                       (0, 16),
            //                       (0, 17)
            //                   }, new List<int?>
            //                      {
            //                          EmptyOrNonSolid, Dirt, null,
            //                          EmptyOrNonSolid, null, Dirt,
            //                          EmptyOrNonSolid, Dirt, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (1, 15),
            //                       (1, 16),
            //                       (1, 17)
            //                   }, new List<int?>
            //                      {
            //                          null, Dirt, EmptyOrNonSolid,
            //                          Dirt, null, EmptyOrNonSolid,
            //                          null, Dirt, EmptyOrNonSolid
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (2, 15),
            //                       (3, 15),
            //                       (4, 15)
            //                   }, new List<int?>
            //                      {
            //                          EmptyOrNonSolid, EmptyOrNonSolid, EmptyOrNonSolid,
            //                          Dirt, null, Dirt,
            //                          null, Dirt, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (11, 5),
            //                       (11, 6),
            //                       (11, 7)
            //                   }, new List<int?>
            //                      {
            //                          EmptyOrNonSolid, EmptyOrNonSolid, EmptyOrNonSolid,
            //                          Dirt, null, Dirt,
            //                          null, Dirt, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (11, 8),
            //                       (11, 9),
            //                       (11, 10)
            //                   }, new List<int?>
            //                      {
            //                          EmptyOrNonSolid, EmptyOrNonSolid, EmptyOrNonSolid,
            //                          Dirt, null, Dirt,
            //                          null, Dirt, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (12, 5),
            //                       (12, 6),
            //                       (12, 7)
            //                   }, new List<int?>
            //                      {
            //                          EmptyOrNonSolid, EmptyOrNonSolid, EmptyOrNonSolid,
            //                          Dirt, null, Dirt,
            //                          null, Dirt, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (12, 8),
            //                       (12, 9),
            //                       (12, 10)
            //                   }, new List<int?>
            //                      {
            //                          EmptyOrNonSolid, EmptyOrNonSolid, EmptyOrNonSolid,
            //                          Dirt, null, Dirt,
            //                          null, Dirt, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (8, 10),
            //                       (9, 10),
            //                       (10, 10)
            //                   }, new List<int?>
            //                      {
            //                          EmptyOrNonSolid, EmptyOrNonSolid, EmptyOrNonSolid,
            //                          Dirt, null, Dirt,
            //                          null, Dirt, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (6, 11),
            //                       (7, 11),
            //                       (8, 11)
            //                   }, new List<int?>
            //                      {
            //                          EmptyOrNonSolid, EmptyOrNonSolid, EmptyOrNonSolid,
            //                          Dirt, null, Dirt,
            //                          null, Dirt, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (9, 11),
            //                       (10, 11),
            //                       (11, 11)
            //                   }, new List<int?>
            //                      {
            //                          EmptyOrNonSolid, EmptyOrNonSolid, EmptyOrNonSolid,
            //                          Dirt, null, Dirt,
            //                          null, Dirt, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (6, 12),
            //                       (6, 13),
            //                       (6, 14)
            //                   }, new List<int?>
            //                      {
            //                          EmptyOrNonSolid, EmptyOrNonSolid, EmptyOrNonSolid,
            //                          Dirt, null, Dirt,
            //                          null, Dirt, null
            //                      });
            //
            //
            // // RegisterTileGroup(new[]
            // //                   {
            // //                       (1, 0),
            // //                       (2, 0),
            // //                       (3, 0)
            // //                   }, new List<int?>
            // //                      {
            // //                          null, Dirt, null,
            // //                          Dirt, null, Dirt,
            // //                          EmptyOrNonSolid, EmptyOrNonSolid, EmptyOrNonSolid
            // //                      });
            //
            // }
            //
            // RegisterTileGroup(new[]
            //                   {
            //                       (0, 0),
            //                       (0, 1),
            //                       (0, 2)
            //                   }, new List<int?>
            //                      {
            //                          null, AnySolid, null,
            //                          EmptyOrNonSolid, null, AnySolid,
            //                          null, AnySolid, null
            //                      }); 
            // RegisterTileGroup(new[]
            //                   {
            //                       (1, 0),
            //                       (2, 0),
            //                       (3, 0)
            //                   }, new List<int?>
            //                      {
            //                          null, EmptyOrNonSolid, null,
            //                          AnySolid, null, AnySolid,
            //                          null, AnySolid, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (1, 2),
            //                       (2, 2),
            //                       (3, 2)
            //                   }, new List<int?>
            //                      {
            //                          null, AnySolid, null,
            //                          AnySolid, null, AnySolid,
            //                          null, EmptyOrNonSolid, null
            //                      });
            //
            // RegisterTileGroup(new[]
            //                   {
            //                       (4, 0),
            //                       (4, 1),
            //                       (4, 2)
            //                   }, new List<int?>
            //                      {
            //                          null, AnySolid, null,
            //                          AnySolid, null, EmptyOrNonSolid,
            //                          null, AnySolid, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (1, 1),
            //                       (2, 1),
            //                       (3, 1)
            //                   }, new List<int?>
            //                      {
            //                          null, This, null,
            //                          This, null, This,
            //                          null, This, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (5, 0),
            //                       (5, 1),
            //                       (5, 2)
            //                   }, new List<int?>
            //                      {
            //                          null, This, null,
            //                          EmptyOrNonSolid, null, EmptyOrNonSolid,
            //                          null, This, null
            //                      }, RuleTile.TilingRuleOutput.Transform.MirrorY);
            // RegisterTileGroup(new[]
            //                   {
            //                       (6, 0),
            //                       (7, 0),
            //                       (8, 0)
            //                   }, new List<int?>
            //                      {
            //                          EmptyOrNonSolid, EmptyOrNonSolid, EmptyOrNonSolid,
            //                          EmptyOrNonSolid, null, EmptyOrNonSolid,
            //                          null, This, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (6, 3),
            //                       (7, 3),
            //                       (8, 3)
            //                   }, new List<int?>
            //                      {
            //                          null, This, null,
            //                          EmptyOrNonSolid, null, EmptyOrNonSolid,
            //                          EmptyOrNonSolid, EmptyOrNonSolid, EmptyOrNonSolid
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (9, 0),
            //                       (9, 1),
            //                       (9, 2)
            //                   }, new List<int?>
            //                      {
            //                          EmptyOrNonSolid, EmptyOrNonSolid, null,
            //                          EmptyOrNonSolid, null, This,
            //                          EmptyOrNonSolid, EmptyOrNonSolid, null
            //                      });
            //
            // RegisterTileGroup(new[]
            //                   {
            //                       (12, 0),
            //                       (12, 1),
            //                       (12, 2)
            //                   }, new List<int?>
            //                      {
            //                          null, EmptyOrNonSolid, EmptyOrNonSolid,
            //                          This, null, EmptyOrNonSolid,
            //                          null, EmptyOrNonSolid, EmptyOrNonSolid
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (9, 3),
            //                       (10, 3),
            //                       (11, 3)
            //                   }, new List<int?>
            //                      {
            //                          null, EmptyOrNonSolid, null,
            //                          EmptyOrNonSolid, null, EmptyOrNonSolid,
            //                          null, EmptyOrNonSolid, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (6, 4),
            //                       (7, 4),
            //                       (8, 4)
            //                   }, new List<int?>
            //                      {
            //                          null, EmptyOrNonSolid, null,
            //                          This, null, This,
            //                          null, EmptyOrNonSolid, null
            //                      }, RuleTile.TilingRuleOutput.Transform.MirrorX);
            // //Corner start
            // RegisterTileGroup(new[]
            //                   {
            //                       (0, 3),
            //                       (2, 3),
            //                       (4, 3)
            //                   }, new List<int?>
            //                      {
            //                          EmptyOrNonSolid, EmptyOrNonSolid, null,
            //                          EmptyOrNonSolid, null, AnySolid,
            //                          null, AnySolid, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (1, 3),
            //                       (3, 3),
            //                       (5, 3)
            //                   }, new List<int?>
            //                      {
            //                          null, EmptyOrNonSolid, EmptyOrNonSolid,
            //                          AnySolid, null, EmptyOrNonSolid,
            //                          null, AnySolid, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (0, 4),
            //                       (2, 4),
            //                       (4, 4)
            //                   }, new List<int?>
            //                      {
            //                          null, AnySolid, null,
            //                          EmptyOrNonSolid, null, AnySolid,
            //                          EmptyOrNonSolid, EmptyOrNonSolid, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (1, 4),
            //                       (3, 4),
            //                       (5, 4)
            //                   }, new List<int?>
            //                      {
            //                          null, AnySolid, null,
            //                          AnySolid, null, EmptyOrNonSolid,
            //                          null, EmptyOrNonSolid, EmptyOrNonSolid
            //                      });
            // Test = false;
            // var assetPath = $"Assets/GeneratedRuleTile{texture.name}.asset";
            // UnityEditor.AssetDatabase.CreateAsset(RuleTile, assetPath);
            // UnityEditor.AssetDatabase.SaveAssets();
            // RegisterTileGroup(new[]
            //                   {
            //                     
            //                   }, new List<int?>
            //                      {});
            // RegisterTileGroup(new[]
            //                   {
            //                       (1, 0),
            //                       (2, 0),
            //                       (3, 0)
            //                   }, new List<int?>
            //                      {
            //                          null, EmptyOrNonSolid, null,
            //                          AnySolid, null, AnySolid,
            //                          null, null, null
            //                      });
            // //Aim Bot
            // RegisterTileGroup(new[]
            //                   {
            //                       (1, 2),
            //                       (2, 2),
            //                       (3, 2)
            //                   }, new List<int?>
            //                      {
            //                          null, null, null,
            //                          AnySolid, null, AnySolid,
            //                          null, EmptyOrNonSolid, null
            //                      });
            // //Aim Right
            // RegisterTileGroup(new[]
            //                   {
            //                       (4, 0),
            //                       (5, 1),
            //                       (6, 2)
            //                   }, new List<int?>
            //                      {
            //                          null, AnySolid, null,
            //                          null, null, EmptyOrNonSolid,
            //                          null, AnySolid, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (1, 1),
            //                       (2, 1),
            //                       (3, 1)
            //                   }, new List<int?>
            //                      {
            //                          null, AnySolid, null,
            //                          AnySolid, null, AnySolid,
            //                          null, AnySolid, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (4, 0),
            //                       (5, 1),
            //                       (6, 2)
            //                   }, new List<int?>
            //                      {
            //                          null, AnySolid, null,
            //                          null, null, EmptyOrNonSolid,
            //                          null, AnySolid, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (6, 0),
            //                       (6, 1),
            //                       (6, 2)
            //                   }, new List<int?>
            //                      {
            //                          null, This, null,
            //                          EmptyOrNonSolid, null, EmptyOrNonSolid,
            //                          null, This, null
            //                      }, RuleTile.TilingRuleOutput.Transform.MirrorY);
            //
            // RegisterTileGroup(new[]
            //                   {
            //                       (6, 0),
            //                       (7, 0),
            //                       (8, 0)
            //                   }, new List<int?>
            //                      {
            //                          null, EmptyOrNonSolid, null,
            //                          EmptyOrNonSolid, null, EmptyOrNonSolid,
            //                          null, This, null
            //                      });
            //
            // RegisterTileGroup(new[]
            //                   {
            //                       (6, 3),
            //                       (7, 3),
            //                       (8, 3)
            //                   }, new List<int?>
            //                      {
            //                          null, This, null,
            //                          EmptyOrNonSolid, null, EmptyOrNonSolid,
            //                          null, EmptyOrNonSolid, null
            //                      });
            //
            //
            // RegisterTileGroup(new[]
            //                   {
            //                       (6, 4),
            //                       (7, 4),
            //                       (8, 4)
            //                   }, new List<int?>
            //                      {
            //                          null, EmptyOrNonSolid, null,
            //                          This, null, This,
            //                          null, EmptyOrNonSolid, null
            //                      }, RuleTile.TilingRuleOutput.Transform.MirrorX);
            //
            // RegisterTileGroup(new[]
            //                   {
            //                       (9, 0),
            //                       (9, 1),
            //                       (9, 2)
            //                   }, new List<int?>
            //                      {
            //                          null, EmptyOrNonSolid, null,
            //                          EmptyOrNonSolid, null, This,
            //                          null, EmptyOrNonSolid, null
            //                      });
            //
            // RegisterTileGroup(new[]
            //                   {
            //                       (12, 0),
            //                       (12, 1),
            //                       (12, 2)
            //                   }, new List<int?>
            //                      {
            //                          null, EmptyOrNonSolid, null,
            //                          This, null, EmptyOrNonSolid,
            //                          null, EmptyOrNonSolid, null
            //                      });
            //
            // RegisterTileGroup(new[]
            //                   {
            //                       (9, 3),
            //                       (10, 3),
            //                       (11, 3)
            //                   }, new List<int?>
            //                      {
            //                          null, EmptyOrNonSolid, null,
            //                          EmptyOrNonSolid, null, EmptyOrNonSolid,
            //                          null, EmptyOrNonSolid, null
            //                      });
            //
            // RegisterTileGroup(new[]
            //                   {
            //                       (0, 3),
            //                       (2, 3),
            //                       (4, 3)
            //                   }, new List<int?>
            //                      {
            //                          null, EmptyOrNonSolid, null,
            //                          EmptyOrNonSolid, null, AnySolid,
            //                          null, AnySolid, null
            //                      });
            //
            // RegisterTileGroup(new[]
            //                   {
            //                       (1, 3),
            //                       (3, 3),
            //                       (5, 3)
            //                   }, new List<int?>
            //                      {
            //                          null, EmptyOrNonSolid, null,
            //                          AnySolid, null, EmptyOrNonSolid,
            //                          null, AnySolid, null
            //                      });
            //
            // RegisterTileGroup(new[]
            //                   {
            //                       (0, 4),
            //                       (2, 4),
            //                       (4, 4)
            //                   }, new List<int?>
            //                      {
            //                          null, AnySolid, null,
            //                          EmptyOrNonSolid, null, AnySolid,
            //                          null, EmptyOrNonSolid, null
            //                      });
            //
            // RegisterTileGroup(new[]
            //                   {
            //                       (1, 4),
            //                       (3, 4),
            //                       (4, 4)
            //                   }, new List<int?>
            //                      {
            //                          null, AnySolid, null,
            //                          AnySolid, null, EmptyOrNonSolid,
            //                          null, EmptyOrNonSolid, null
            //                      });
            // if (!mergeWithDirt)
            //     return;
            //
            // RegisterTileGroup(new[]
            //                   {
            //                       (2, 5),
            //                       (2, 7),
            //                       (2, 9)
            //                   }, new List<int?>
            //                      {
            //                          null, AnySolid, null,
            //                          AnySolid, null, AnySolid,
            //                          null, AnySolid, EmptyOrNonSolid
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (2, 6),
            //                       (2, 8),
            //                       (2, 10)
            //                   }, new List<int?>
            //                      {
            //                          null, AnySolid, EmptyOrNonSolid,
            //                          AnySolid, null, AnySolid,
            //                          null, AnySolid, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (3, 5),
            //                       (3, 7),
            //                       (3, 9)
            //                   }, new List<int?>
            //                      {
            //                          null, AnySolid, null,
            //                          AnySolid, null, AnySolid,
            //                          EmptyOrNonSolid, AnySolid, null
            //                      });
            // RegisterTileGroup(new[]
            //                   {
            //                       (3, 6),
            //                       (3, 8),
            //                       (3, 10)
            //                   }, new List<int?>
            //                      {
            //                          EmptyOrNonSolid, AnySolid, null,
            //                          AnySolid, null, AnySolid,
            //                          null, AnySolid, null
            //                      });
        }
    }
}