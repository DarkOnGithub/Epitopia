namespace Events.EventHandler
{
    public interface IEvent
    {
        public bool IsCancelled { get; set; }
    }
}