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
                        Album = t.Album?.Title,
                        AlbumCover = t.Album is not null && t.Album.Thumb is not null ? 
                            new AlbumCoverInfo(t.Album.Thumb.Height, t.Album.Thumb.Width, t.Album.Thumb.Photo300) : 
                            null,
                        Length = TimeSpan.FromSeconds(t.Duration),
                        IsHQ = t.IsHq ?? default,
                        UID = Guid.NewGuid(),
                        Url = t.Url?.ToString(),
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

        #endregion
    }
}
