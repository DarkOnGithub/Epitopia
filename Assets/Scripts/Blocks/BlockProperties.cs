using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Blocks
{
    public class BlockProperties
    {
        [CanBeNull] public string SpritePath;   
        public bool IsCollidable = true;
        public int LightStrength = 0;
        
        [CanBeNull]
        public Tile GetTile()
        {
            if (SpritePath == null)
                return null;
            var tile =  ScriptableObject.CreateInstance<Tile>();
            tile.sprite = Resources.Load<Sprite>(SpritePath);
            return tile;
        }
    }
}