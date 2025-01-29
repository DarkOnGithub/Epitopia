using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Tiles
{
    public class FlowerTile : AbstractTile
    {
        public FlowerTile(string path) : base()
        {
            var sprites = Resources.LoadAll<Sprite>(path);
            RegisterTileFromSprites(new()
                                    {
                                        null, null, null,
                                        null, null, null,
                                        null, AnySolid, null
                                    }, sprites.ToList());

            AssetDatabase.CreateAsset(RuleTile, "Assets/TemporaryRuleTile.asset");
            AssetDatabase.SaveAssets();
        }
    }
}