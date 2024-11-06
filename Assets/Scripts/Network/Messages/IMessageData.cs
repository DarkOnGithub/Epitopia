namespace Network.Messages
{
    public interface IMessageData
    {
        
    }

    public interface IMessageVariable<T> : IMessageData
    {
        T Message { get; set; }
    }
}