using System;

namespace test_wpf1.Internals.Exceptions
{
    public class TrackDownloadingException : Exception
    {
        public TrackDownloadingException() : base() { }

        public TrackDownloadingException(string message) : base(message) { }
    }
}
