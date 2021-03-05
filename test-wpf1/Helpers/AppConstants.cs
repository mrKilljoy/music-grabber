namespace test_wpf1.Helpers
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

        public static class Statuses
        {
            public const string PreparingTitle = "preparing...";
            public const string AuthenticationTitle = "authentication...";
            public const string RetrievingDataTitle = "getting...";
        }

        public static class Messages
        {
            public const string ExistingFileMessage = "The file already exists.\nOverwrite?";
            public const string MissingConfigurationFileErrorMessage = "No configuration has been found.\nPress 'OK' to exit the app.";
            public const string UnknownErrorMessage = "An unknown error has occurred.\nPress 'OK' to exit the app.";
            public const string UnknownErrorWithDetailsMessage = "An unknown error has occurred.\nDetails: {0}";
        }

        public static class Metadata
        {
            public const string ErrorMessageField = "errorMessage";
        }
    }
}
