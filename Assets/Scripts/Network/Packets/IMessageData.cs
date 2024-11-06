namespace Network.Packets
{
    public interface IMessageData
    {
        
    }

    public interface IMessageVariable<T> : IMessageData
    {
        T Message { get; set; }
    }
}