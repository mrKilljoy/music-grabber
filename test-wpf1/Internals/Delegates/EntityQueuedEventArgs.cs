using System;
using test_wpf1.Contracts;

namespace test_wpf1.Internals.Delegates
{
    public class EntityQueuedEventArgs : EventArgs
    {
        public EntityQueuedEventArgs(IQueryableEntity track) => this.Track = track;

        public IQueryableEntity Track { get; }
    }
}
