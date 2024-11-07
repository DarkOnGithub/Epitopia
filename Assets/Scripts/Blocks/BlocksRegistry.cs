namespace Blocks
{
    public static class BlocksRegistry
    {
        public static DefaultBlock BLOCK_AIR;
        public static DefaultBlock BLOCK_DIRT;
        static BlocksRegistry()
        {
            BLOCK_AIR = new DefaultBlock("Air", new BlockProperties()
                                                {
                                                    IsCollidable = false 
                                                });
            BLOCK_DIRT = new DefaultBlock("Dirt", new BlockProperties()
                                                {
                                                    SpritePath = "Sprites/Blocks/Dirt"
                                                });
        }
    }
}