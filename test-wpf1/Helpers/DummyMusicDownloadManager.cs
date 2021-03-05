using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using test_wpf1.Configuration;
using test_wpf1.Contracts;
using test_wpf1.Models;

namespace test_wpf1.Helpers
{
    public sealed class DummyMusicDownloadManager : IMusicDownloadManager
    {
        private readonly DownloadSettingsSection configSection;

        public DummyMusicDownloadManager(IOptions<DownloadSettingsSection> options)
        {
            if (options.Value == null)
                throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrEmpty(options.Value.OutputFolder))
            {
                throw new ArgumentException(nameof(options.Value.OutputFolder));
            }

            this.configSection = options.Value;
        }

        public Task<TrackDownloadResult> DownloadAsync(Track track, bool overwrite)
        {
            var filename = BuildFileName(track);

            if (File.Exists(filename))
            {
                if (!overwrite)
                {
                    return Task.FromResult(new TrackDownloadResult(false, new Dictionary<string, object>
                    {
                        { "errorMessage", "a file with the same name already exists" }
                    }));
                }
                else
                {
                    File.Delete(filename);
                }
            }

            using (var newFile = File.Create(filename))
            {
                newFile.WriteByte(1);
                newFile.Flush();
            }

            return Task.FromResult(new TrackDownloadResult(true));
        }

        private string BuildFileName(Track track)
        {
            return Path.Combine(this.configSection.OutputFolder, $"{track.Artist} - {track.Title}.{track.Extension}");
        }
    }
}
