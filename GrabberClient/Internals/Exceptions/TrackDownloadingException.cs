using System;

namespace GrabberClient.Internals.Exceptions
{
    public class TrackDownloadingException : Exception
    {
        public TrackDownloadingException() : base() { }

        public TrackDownloadingException(string message) : base(message) { }
    }
}
