using System;
using System.Linq;
using QFSW.QC;
using Tiles;
using UnityEditor;
using UnityEngine;

namespace Core.Commands
{
    public static class TestCommands
    {
        [Command]
        public static void ExportTile(string tileName)
        {
            var b = "";
            var rule = Resources.Load<RuleTile>(tileName);
            var i = 0;
            foreach (var tilingRule in rule.m_TilingRules)
            {
                var rules = new int[9];
                for (var j = 0; j < tilingRule.m_NeighborPositions.Count; j++)
                {
                    var r = tilingRule.m_NeighborPositions[j];
                    var k = AbstractTile.LookUpTable.IndexOf(r);
                    rules[k] = tilingRule.m_Neighbors[j];
                }

                var sprites = AbstractTile.Temp[i];
                b += @$"
        RegisterTileGroup(new[]
        {{
            {string.Join(",\n            ", sprites.Select(p => $"({p.Item1},{p.Item2})"))}
        }}, new List<int?>
        {{
            {string.Join(",\n            ", rules.Select(r => r == 0 ? "null" : r.ToString()))}
        }});
        \n";
                i++;
            }

            Debug.Log(b);
        }
    }
}