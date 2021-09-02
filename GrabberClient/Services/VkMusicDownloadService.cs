using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GrabberClient.Contracts;
using GrabberClient.Helpers;
using GrabberClient.Models;
using GrabberClient.Internals.Exceptions;
using GrabberClient.Configuration;
using GrabberClient.Internals;
using System.Linq;

namespace GrabberClient.Services
{
    public sealed class VkMusicDownloadService : IMusicDownloadService
    {
        #region Fields

        private readonly Regex filenamePattern = new Regex("^([A-Za-z0-9 \\&\\-\\]){1,}\\(([0-9]){1,}\\).mp3$");

        private readonly DownloadSettingsSection settings;

        #endregion

        #region .ctr

        public VkMusicDownloadService(IOptions<DownloadSettingsSection> options)
        {
            this.settings = options.Value;
        }

        #endregion

        #region Methods

        public async Task<EntityDownloadResult> DownloadAsync(Track track)
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    if (string.IsNullOrEmpty(track.Url))
                        throw new VkTrackMissingUrlException(track);

                    string trackName = this.BuildTrackName(track);
                    string trackUrl = this.ParseTrackUri(track.Url);

                    await webClient.DownloadFileTaskAsync(trackUrl, trackName).ConfigureAwait(false);

                    return new EntityDownloadResult(true, new Dictionary<string, object>
                    {
                        [AppConstants.Metadata.UidField] = track.UID,
                        [AppConstants.Metadata.TrackPathField] = trackName
                    });
                }
                catch (Exception ex)
                {
                    return new EntityDownloadResult(false, new Dictionary<string, object>
                    {
                        [AppConstants.Metadata.MessageField] = ex.Message,
                        [AppConstants.Metadata.ExceptionField] = ex
                    });
                }
            }
        }

        private string BuildTrackName(Track track)
        {
            string trackFilename = $"{track.Artist} - {track.Title}.{track.Extension}";
            string trackFolderPath = this.settings.OutputFolder;
            string fullTrackPath = Path.Combine(trackFolderPath, trackFilename);

            if (!File.Exists(fullTrackPath))
                return fullTrackPath;

            int lastNumber = default(int);
            var dirFiles = Directory.GetFiles(trackFolderPath);

            foreach (var filePath in dirFiles)
            {
                var filename = Path.GetFileName(filePath);

                if (this.filenamePattern.IsMatch(filename))
                {
                    int lpIndex = filename.LastIndexOf('(');
                    int rpIndex = filename.LastIndexOf(')');
                    int numberLength = rpIndex - lpIndex - 1;
                    string numberString = filename.Substring(lpIndex + 1, numberLength);
                    if (int.TryParse(numberString, out int number) && number > lastNumber)
                    {
                        lastNumber = number;
                    }
                }
            }

            trackFilename = $"{track.Artist} - {track.Title} ({lastNumber + 1}).{track.Extension}";
            fullTrackPath = Path.Combine(trackFolderPath, trackFilename);

            return fullTrackPath;
        }

        private string ParseTrackUri(string trackUrl)
        {
            var trackUri = new Uri(trackUrl);
            var segments = trackUri.Segments.ToList();
            segments.RemoveAt((segments.Count - 1) / 2);
            segments.RemoveAt(segments.Count - 1);

            segments[segments.Count - 1] = segments[segments.Count - 1].Replace("/", ".mp3");

            return $"{trackUri.Scheme}://{trackUri.Host}{string.Join(string.Empty, segments)}{trackUri.Query}";
        }

        #endregion
    }
}
