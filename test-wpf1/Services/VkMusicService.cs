using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test_wpf1.Contracts;
using test_wpf1.Helpers;
using test_wpf1.Models;
using VkNet;
using VkNet.Model.RequestParams;

namespace test_wpf1.Services
{
    /// <summary>
    /// A service providing access to audio tracks from VK.net
    /// </summary>
    public sealed class VkMusicService : IMusicService
    {
        private readonly VkApi api;

        public VkMusicService(VkApi api) => this.api = api;

        public async Task<IEnumerable<Track>> GetTracksAsync(string query)
        {
            List<Track> results = new List<Track>();

            try
            {
                var found = await this.api.Audio.SearchAsync(new AudioSearchParams
                {
                    Query = query,
                    Count = 30
                });

                if (found != null && found.Count > 0)
                {
                    results.AddRange(found.Select(t => new Track
                    {
                        Title = t.Title,
                        Artist = t.Artist,
                        Length = TimeSpan.FromSeconds(t.Duration),
                        IsHQ = t.IsHq ?? default,
                        UID = Guid.NewGuid(),
                        Url = t.Url != null ? ParseTrackUri(t.Url) : null,
                        Extension = AppConstants.AudioTrackCommonFileExtension
                    }));
                }
            }
            catch (Exception)
            {
                throw;
            }

            return results;
        }

        private string ParseTrackUri(Uri trackUri)
        {
            var segments = trackUri.Segments.ToList();
            segments.RemoveAt((segments.Count - 1) / 2);
            segments.RemoveAt(segments.Count - 1);

            segments[segments.Count - 1] = segments[segments.Count - 1].Replace("/", ".mp3");

            return $"{trackUri.Scheme}://{trackUri.Host}{string.Join(string.Empty, segments)}{trackUri.Query}";
        }
    }
}
