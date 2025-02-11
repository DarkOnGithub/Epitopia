using MessagePack;
using Tiles;
using UnityEngine;

namespace World.Blocks.CustomBlocks
{
    [MessagePackObject]
    public struct DefaultBlockState : IBlockState
    {
        [Key(0)]public int LightLevel { get; set; }
        [Key(1)]public byte WallId { get; set; }
        [Key(2)]public int Id { get; set; }
        
    }
    public class DefaultBlock : AbstractBlock
    {
        public DefaultBlock(int id, string name, BlockProperties properties) : base(id, name, properties)
        {
            Tile = new DefaultTile(properties.SpritePath, properties.MergeWithDirt).RuleTile;
        }

        public override IBlockState CreateBlockState(int? state = null, byte wallId = 0, int lightLevel = 0) => new DefaultBlockState
        {
            Id = BlockId, 
            WallId = wallId, 
            LightLevel = lightLevel
        };
    }
}