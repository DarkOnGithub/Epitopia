namespace Events.EventHandler
{
    public interface ICancellable
    {
        bool IsCanceled()
        {
            return ((IEvent)this).IsCancelled;
        }

        void SetCanceled(bool canceled)
        {
            ((IEvent)this).IsCancelled = canceled;
        }
    }
}