using System.Linq;
using Tiles;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class RuleTileExport : EditorWindow
    {
        [MenuItem("Assets/Export Rule Tile as Template")]
        public static void ShowWindow()
        {
            var selection = (DefaultRuleTile)Selection.activeObject;
            if(selection == null) return;
            GUIUtility.systemCopyBuffer = (ExportRuleTile(selection));
        }
        [MenuItem("Assets/Export Rule Tile as Template", true)]
        private static bool Validate()
        {
            return Selection.activeObject is DefaultRuleTile;
        }


        private static string ExportRuleTile(DefaultRuleTile ruleTile)
        {
            var text = "";
             foreach (var tilingRule in ruleTile.m_TilingRules)
             {
                 var rules = new int[9];
                 for (var j = 0; j < tilingRule.m_NeighborPositions.Count; j++)
                 {
                     var r = tilingRule.m_NeighborPositions[j];
                     var k = AbstractTile.LookUpTable.IndexOf(r);
                     rules[k] = tilingRule.m_Neighbors[j];
                 }

                 var sprites = tilingRule.m_Sprites;
                 text += @$"
                        RegisterTileGroup(new[]
                        {{
                            {string.Join(",\n                ", sprites.Select(p => p.name))}
                        }}, new List<int?>
                        {{
                            {string.Join(",\n                ", rules.Select(r => r == 0 ? "null" : r.ToString()))}
                        }});";
             }

             return text;                   
         //         var sprites = AbstractTile.Temp[i];
         //         b += @$"
         // RegisterTileGroup(new[]
         // {{
         //     {string.Join(",\n            ", sprites.Select(p => $"({p.Item1},{p.Item2})"))}
         // }}, new List<int?>
         // {{
         //     {string.Join(",\n            ", rules.Select(r => r == 0 ? "null" : r.ToString()))}
         // }});
         // \n";
         //         i++;
         //     }

        }
    }
}