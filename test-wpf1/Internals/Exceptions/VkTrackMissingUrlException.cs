using test_wpf1.Models;

namespace test_wpf1.Internals.Exceptions
{
    public sealed class VkTrackMissingUrlException : TrackDownloadingException
    {
        public VkTrackMissingUrlException(Track track) 
        {
            this.Track = track;
        }

        public Track Track { get; }
    }
}
