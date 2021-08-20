using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GrabberClient.Contracts;
using GrabberClient.Configuration;
using GrabberClient.Helpers;
using GrabberClient.Models;
using GrabberClient.Internals.Exceptions;

namespace GrabberClient.Services
{
    public sealed class DummyMusicDownloadService : IMusicDownloadService
    {
        private readonly DownloadSettingsSection configSection;
        private bool isBusy;

        private readonly object syncAnchor = new object();

        public DummyMusicDownloadService(IOptions<DownloadSettingsSection> options)
        {
            if (options.Value is null)
                throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrEmpty(options.Value.OutputFolder))
            {
                throw new ArgumentException(nameof(options.Value.OutputFolder));
            }

            this.configSection = options.Value;
        }

        public bool IsBusy => this.isBusy;

        public Task<TrackDownloadResult> DownloadAsync(Track track)
        {
            if (IsBusy)
            {
                return Task.FromResult(new TrackDownloadResult(
                    false,
                    new Dictionary<string, object>
                    {
                        ["cancelationReason"] = "isBusy"
                    }));
            }

            var filename = BuildFileName(track);

            if (File.Exists(filename))
            {
                return Task.FromResult(new TrackDownloadResult(false, new Dictionary<string, object>
                {
                    ["errorMessage"] = "a file with the same name already exists"
                }));
            }

            lock (syncAnchor)
            {
                try
                {
                    this.isBusy = true;

                    using (var newFile = File.Create(filename))
                    {
                        newFile.WriteByte(1);
                        newFile.Flush();
                    }

                    Thread.Sleep(3000);

                    return Task.FromResult(new TrackDownloadResult(
                        true, new Dictionary<string, object>
                        {
                            [nameof(track.UID)] = track.UID
                        }));
                }
                catch (Exception)
                {
                    throw new TrackDownloadingException();
                }
                finally
                {
                    this.isBusy = false;
                }
            }
        }

        private string BuildFileName(Track track)
        {
            return Path.Combine(this.configSection.OutputFolder, $"{track.Artist} - {track.Title}.{track.Extension}");
        }
    }
}
