using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using test_wpf1.Contracts;
using test_wpf1.Delegates;

namespace test_wpf1.Helpers
{
    public sealed class DownloadQueueManager : IQueueManager
    {
        private const double TimerInterval = 1000;

        public event TrackQueuedEventHandler QueueAdded;
        public event TrackQueuedEventHandler QueueRemoved;
        private event EventHandler TriggeredQueue;

        private readonly Queue<IQueryableEntity> queue = new Queue<IQueryableEntity>();
        private readonly Timer timer;

        private readonly static object syncObject = new object();

        public DownloadQueueManager()
        {
            this.timer = new Timer(TimerInterval);

            this.TriggeredQueue += ProcessTrackAsync;
        }

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
            this.TriggeredQueue?.Invoke(this, EventArgs.Empty);
        }

        //  Method simulates working process and does nothing else.
        private async void ProcessTrackAsync(object obj, EventArgs args)
        {
            //  do something with the track
            await Task.Delay(1000);
        }
    }
}
