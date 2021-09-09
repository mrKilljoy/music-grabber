﻿namespace GrabberClient.Internals
{
    /// <summary>
    /// A class with constant values for the application.
    /// </summary>
    public static class AppConstants
    {
        public const int QueryDelayInMilliseconds = 1000;
        public const string ConfigurationFileName = "appsettings.json";
        public const string ConfigurationCredentialsSectionName = "credentials";
        public const string ConfigurationDownloadSettingsSectionName = "downloadSettings";
        public const string AudioTrackCommonFileExtension = "mp3";

        public static class Statuses
        {
            public const string PreparingTitle = "...preparing";
            public const string AuthenticationTitle = "...authorizing";
            public const string RetrievingDataTitle = "...retrieving data";
        }

        public static class Messages
        {
            public const string ExistingFileMessage = "The file already exists.\nOverwrite?";
            public const string MissingConfigurationFileErrorMessage = "No configuration has been found.\nPress 'OK' to exit the app.";
            public const string UnknownErrorMessage = "An unknown error has occurred.\nPress 'OK' to exit the app.";
            public const string UnknownErrorWithDetailsMessage = "An unknown error has occurred.\nDetails: {0}";
            public const string NoTrackSelectedMessage = "There are no selected tracks to download.";
            public const string AuthorizationFailureMessage = "Authorization was not successful";
            public const string CaptchaRequiredMessage = "Captcha prompt is required!";
            public const string DownloadCompleteMessage = "Download complete";
        }

        public static class Metadata
        {
            public const string MessageField = ".message";
            public const string ErrorMessageField = ".errorMessage";
            public const string ExceptionField = ".exception";
            public const string UidField = ".uid";
            public const string ApiField = ".api";
            public const string TrackPathField = ".trackPath";
            public const string TrackAlbumCoverBytesField = ".albumCoverBytes";
        }
    }
}
