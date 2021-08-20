using System.Collections.Generic;
using test_wpf1.Contracts;
using test_wpf1.Internals.Delegates;

namespace test_wpf1.Services
{
    public sealed class DownloadQueueService : IQueueService
    {
        #region Fields

        public event TrackQueuedEventHandler QueueAdded;
        public event TrackQueuedEventHandler QueueRemoved;

        private readonly Queue<IQueryableEntity> queue = new Queue<IQueryableEntity>();

        #endregion

        #region Methods

        public int Count() => this.queue.Count;

        public IQueryableEntity Peek() => this.queue.Peek();

        public IQueryableEntity Dequeue()
        {
            var dq = this.queue.Dequeue();
            this.QueueRemoved?.Invoke(this, new EntityQueuedEventArgs(dq));
            return dq;
        }

        public void Enqueue(IQueryableEntity track)
        {
            this.queue.Enqueue(track);
            this.QueueAdded?.Invoke(this, new EntityQueuedEventArgs(track));
        }

        #endregion
    }
}
