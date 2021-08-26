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

        public async Task<TrackDownloadResult> DownloadAsync(Track track)
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    if (string.IsNullOrEmpty(track.Url))
                        throw new VkTrackMissingUrlException(track);

                    string trackName = BuildTrackName(track);

                    await webClient.DownloadFileTaskAsync(track.Url, trackName).ConfigureAwait(false);

                    return new TrackDownloadResult(true, new Dictionary<string, object>
                    {
                        [AppConstants.Metadata.UidField] = track.UID
                    });
                }
                catch (Exception ex)
                {
                    return new TrackDownloadResult(false, new Dictionary<string, object>
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

        #endregion
    }
}
