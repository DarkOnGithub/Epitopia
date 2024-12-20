using UnityEngine;
using UnityEngine.Tilemaps;

namespace Utils
{
    public static class SpriteUtils
    {
        public static Tile GetTileFromSprite(Sprite sprite)
        {
            var tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = sprite;
            return tile;
        }
    }
}