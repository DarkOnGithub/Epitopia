namespace Blocks
{
    public interface IBlock
    {
        int Id { get; }
        string Name { get; }
        
        //IBlockData CreateBlock();
    }
}