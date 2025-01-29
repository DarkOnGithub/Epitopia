using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tiles
{
    public abstract class AbstractTile
    {
        protected const int This = RuleTile.TilingRuleOutput.Neighbor.This;
        protected const int NotThis = RuleTile.TilingRuleOutput.Neighbor.NotThis;
        protected const int Any = DefaultRuleTile.Neighbors.Any;          // 9
        protected const int Empty = DefaultRuleTile.Neighbors.Empty;      // 10
        protected const int Dirt = DefaultRuleTile.Neighbors.Dirt;        // 7 (was OnlyDirt)
        protected const int Different = DefaultRuleTile.Neighbors.Different;    // 11
        protected const int ThisOrWall = DefaultRuleTile.Neighbors.ThisOrWall;  // 12
        protected const int AnySolid = DefaultRuleTile.Neighbors.AnySolid;      // 4
        protected const int AnySolidNotDirt = DefaultRuleTile.Neighbors.AnySolidNotDirt;  // 5
        protected const int EmptyOrNonSolid = DefaultRuleTile.Neighbors.EmptyOrNonSolid;  // 6
        protected const int Wall = DefaultRuleTile.Neighbors.Wall;        // 8
        
        private static readonly List<Vector3Int> LookUpTable = new()
                                                               {
                                                                   new Vector3Int(-1, 1, 0),
                                                                   new Vector3Int(0, 1, 0),
                                                                   new Vector3Int(1, 1, 0),
                                                                   new Vector3Int(-1, 0, 0),
                                                                   new Vector3Int(0, 0, 0),
                                                                   new Vector3Int(1, 0, 0),
                                                                   new Vector3Int(-1, -1, 0),
                                                                   new Vector3Int(0, -1, 0),
                                                                   new Vector3Int(1, -1, 0)
                                                               };

        public readonly RuleTile RuleTile = ScriptableObject.CreateInstance<DefaultRuleTile>();
        protected Dictionary<Vector2Int, Sprite> Sprites;
        
        protected AbstractTile()
        {
        }
       
        protected AbstractTile(Texture2D texture)
        {
            Sprites = SplitTexture(texture);
        }

        protected virtual Vector2Int Padding { get; } = new(2, 2);
        protected virtual Vector2Int Size { get; } = new(16, 16);
        protected virtual int PixelsPerUnit { get; } = 16;

        protected Dictionary<Vector2Int, Sprite> SplitTexture(Texture2D texture)
        {
            if (texture == null) return new Dictionary<Vector2Int, Sprite>();
            var result = new Dictionary<Vector2Int, Sprite>();
            var yIndex = 0;

            for (var y = 0; y < texture.height; y += Size.y + Padding.y)
            {
                var xIndex = 0;
                for (var x = 0; x < texture.width; x += Size.x + Padding.x)
                {
                    var correctedY = texture.height - (y + Size.y);
                    if (correctedY < 0) break;

                    var pixels = texture.GetPixels(x, correctedY, Size.x, Size.y);

                    if (pixels.All(pixel => pixel.a == 0))
                    {
                        xIndex++;
                        continue; 
                    }

                    var newTexture = new Texture2D(Size.x, Size.y);
                    newTexture.filterMode = FilterMode.Point;
                    newTexture.SetPixels(pixels);
                    newTexture.Apply();

                    var newSprite = Sprite.Create(newTexture, new Rect(0, 0, Size.x, Size.y), new Vector2(0.5f, 0.5f), 16);

                    result[new Vector2Int(xIndex, yIndex)] = newSprite;
                    xIndex++;
                }

                yIndex++;
            }

            return result;
        }


        protected void RegisterTileFromSprites(List<int?> neighbors, List<Sprite> sprites, RuleTile.TilingRuleOutput.Transform transform = RuleTile.TilingRuleOutput.Transform.Fixed)
        {
            if(sprites.Count == 0) return;
            var neighborPositions = new List<Vector3Int>();
            var neighborsUpdated = new List<int>();

            for (var i = 0; i < neighbors.Count; i++)
            {
                var neighbor = neighbors[i];
                if (neighbor is null || i == 4) continue;
                neighborPositions.Add(LookUpTable[i]);
                neighborsUpdated.Add((int)neighbor);
            }

            var rule = new RuleTile.TilingRule
                       {
                           //m_Neighbors = neighborsUpdated,
                           //m_NeighborPositions = neighborPositions,
                           m_Sprites = sprites.ToArray(),
                           m_RuleTransform = transform
                       };
            if(sprites.Count > 1)
                rule.m_Output = RuleTile.TilingRuleOutput.OutputSprite.Random;
            RuleTile.m_TilingRules.Add(rule);
        }
        protected void RegisterTileGroup((int x, int y)[] coords, List<int?> neighbors,
            RuleTile.TilingRuleOutput.Transform transform = RuleTile.TilingRuleOutput.Transform.Fixed)
        {
            var group = new List<Sprite>();
            foreach (var coord in coords)
            {
                var position = new Vector2Int(
                    coord.x,
                    coord.y
                );

                if (Sprites.ContainsKey(position))
                    group.Add(Sprites[position]);
                else
                    Debug.LogWarning($"Sprite at {position} not found in the sprite dictionary.");
            }
            RegisterTileFromSprites(neighbors, group, transform);
        }
    }
}