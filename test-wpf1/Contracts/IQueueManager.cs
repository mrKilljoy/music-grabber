using System;
using System.Collections.Generic;
using System.Text;
using test_wpf1.Delegates;
using test_wpf1.Models;

namespace test_wpf1.Contracts
{
    public interface IQueueManager
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
