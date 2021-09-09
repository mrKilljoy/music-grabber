namespace GrabberClient.Models
{
    public class AlbumCoverInfo
    {
        public AlbumCoverInfo (long height, long width, string coverUrl)
        {
            this.Height = height;
            this.Width = width;
            this.CoverUrl = coverUrl;
        }

        public long Height { get; set; }

        public long Width { get; set; }

        public string CoverUrl { get; set; }
    }
}
