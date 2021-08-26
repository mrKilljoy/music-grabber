using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrabberClient.Contracts;
using GrabberClient.Internals;
using GrabberClient.Models;
using VkNet;
using VkNet.Model.RequestParams;

namespace GrabberClient.Services
{
    /// <summary>
    /// A service providing access to audio tracks from VK.net
    /// </summary>
    public sealed class VkMusicService : IMusicService
    {
        #region Constants

        private const int QueryResultsLimit = 30;

        #endregion

        #region Fields

        private readonly VkApi api;

        #endregion

        #region .ctr

        public VkMusicService(VkApi api) => this.api = api;

        #endregion

        #region Methods

        public async Task<IEnumerable<Track>> GetTracksAsync(string query)
        {
            List<Track> trackList = new List<Track>();

            try
            {
                var searchResults = await this.api.Audio.SearchAsync(new AudioSearchParams
                {
                    Query = query,
                    Count = QueryResultsLimit
                }).ConfigureAwait(false);

                if (searchResults is not null && searchResults.Count > 0)
                {
                    trackList.AddRange(searchResults.Select(t => new Track
                    {
                        Title = t.Title,
                        Artist = t.Artist,
                        Length = TimeSpan.FromSeconds(t.Duration),
                        IsHQ = t.IsHq ?? default,
                        UID = Guid.NewGuid(),
                        Url = t.Url is not null ? ParseTrackUri(t.Url) : null,
                        Extension = AppConstants.AudioTrackCommonFileExtension
                    }));
                }
            }
            catch (Exception)
            {
                throw;
            }

            return trackList;
        }

        private string ParseTrackUri(Uri trackUri)
        {
            var segments = trackUri.Segments.ToList();
            segments.RemoveAt((segments.Count - 1) / 2);
            segments.RemoveAt(segments.Count - 1);

            segments[segments.Count - 1] = segments[segments.Count - 1].Replace("/", ".mp3");

            return $"{trackUri.Scheme}://{trackUri.Host}{string.Join(string.Empty, segments)}{trackUri.Query}";
        }

        #endregion
    }
}
