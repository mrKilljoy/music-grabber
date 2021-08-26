namespace GrabberClient.Internals.Exceptions
{
    public class TrackDownloadingException : AppException
    {
        public TrackDownloadingException() : base() { }

        public TrackDownloadingException(string message) : base(message) { }
    }
}
