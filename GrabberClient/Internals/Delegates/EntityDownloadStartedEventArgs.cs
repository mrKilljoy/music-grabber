using GrabberClient.Contracts;
using System;

namespace GrabberClient.Internals.Delegates
{
    public class EntityDownloadStartedEventArgs : EventArgs
    {
        public EntityDownloadStartedEventArgs(IQueryableEntity entity) => this.Entity = entity;

        public IQueryableEntity Entity { get; }
    }
}
