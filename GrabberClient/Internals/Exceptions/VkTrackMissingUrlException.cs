using GrabberClient.Models;

namespace GrabberClient.Internals.Exceptions
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
