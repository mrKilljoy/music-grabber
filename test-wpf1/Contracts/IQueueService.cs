using test_wpf1.Delegates;

namespace test_wpf1.Contracts
{
    public interface IQueueService
    {
        #region Events

        event TrackQueuedEventHandler QueueAdded;
        event TrackQueuedEventHandler QueueRemoved;

        #endregion

        #region Methods

        void Enqueue(IQueryableEntity entity);
        IQueryableEntity Dequeue();
        IQueryableEntity Peek();

        int Count();

        #endregion
    }
}
