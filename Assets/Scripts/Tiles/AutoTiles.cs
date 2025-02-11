using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Tiles
{
    public class AutoTiles : AbstractTile
    {
        protected new Sprite[] Sprites;
        public AutoTiles(string path) : base()
        {
            Sprites = Resources.LoadAll<Sprite>($"Sprites/Blocks/{path}");
        }
    }
}