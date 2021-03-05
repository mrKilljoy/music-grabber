using System;
using test_wpf1.Helpers;

namespace test_wpf1.Delegates
{
    public class TrackDownloadEventArgs : EventArgs
    {
        public TrackDownloadEventArgs() : base() => this.Response = new TrackDownloadResult(default(bool));

        public TrackDownloadEventArgs(TrackDownloadResult response) => this.Response = response;

        public TrackDownloadResult Response { get; }
    }
}
