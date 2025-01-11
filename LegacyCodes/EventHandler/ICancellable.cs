namespace Old
{
    public interface ICancellable
    {
        bool IsCanceled()
        {
            return ((Event)this).IsCancelled;
        }

        void SetCanceled(bool canceled)
        {
            ((Event)this).IsCancelled = canceled;
        }
    }
}