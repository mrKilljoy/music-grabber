using System;
using GrabberClient.Helpers;

namespace GrabberClient.Internals.Delegates
{
    public class EntityDownloadCompletedEventArgs : EventArgs
    {
        public EntityDownloadCompletedEventArgs() : base() => this.Response = new EntityDownloadResult(default(bool));

        public EntityDownloadCompletedEventArgs(EntityDownloadResult response) => this.Response = response;

        public EntityDownloadResult Response { get; }
    }
}
