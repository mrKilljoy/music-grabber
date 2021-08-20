using System;
using GrabberClient.Contracts;

namespace GrabberClient.Internals.Delegates
{
    public class EntityQueuedEventArgs : EventArgs
    {
        public EntityQueuedEventArgs(IQueryableEntity track) => this.Track = track;

        public IQueryableEntity Track { get; }
    }
}
