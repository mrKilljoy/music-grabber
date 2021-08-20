using System;
using GrabberClient.Helpers;

namespace GrabberClient.Internals.Delegates
{
    public class TrackDownloadEventArgs : EventArgs
    {
        public TrackDownloadEventArgs() : base() => this.Response = new TrackDownloadResult(default(bool));

        public TrackDownloadEventArgs(TrackDownloadResult response) => this.Response = response;

        public TrackDownloadResult Response { get; }
    }
}
