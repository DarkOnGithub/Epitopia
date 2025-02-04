namespace World.Blocks.CustomBlocks
{
    public struct DefaultBlockState : IBlockState
    {
        public int LightLevel { get; set; }
        public byte WallId { get; set; }
        public int Id { get; }
        public DefaultBlockState(int id, byte wallId, int lightLevel)
        {
            Id = id;
            LightLevel = lightLevel;
            WallId = wallId;
        }
    }
    public class DefaultBlock : AbstractBlock
    {
        public DefaultBlock(int id, string name, BlockProperties properties) : base(id, name, properties)
        {
               
        }

        public override IBlockState CreateBlockState(int? state = null, byte wallId = 0, int lightLevel = 0) => new DefaultBlockState(BlockId, wallId, lightLevel);
    }
}