using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using test_wpf1.Contracts;
using test_wpf1.Delegates;
using test_wpf1.Models;

namespace test_wpf1.ViewModels
{
    public sealed class MainWindowViewModel : IMainViewViewModel
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        public event AuthEventHandler LoginReacted;
        public event TrackDownloadEventHandler DownloadReacted;
        public event EventHandler QueryReacted;
        public event EventHandler LogoutReacted;

        #endregion

        #region Fields

        private readonly IAuthManager authManager;
        private readonly ICredentialsReader credentialsReader;
        private readonly IServiceManager serviceManager;
        private readonly IMusicDownloadManager musicDownloadManager;
        private readonly IMusicService musicService;

        private ObservableCollection<Track> tracks;
        private Track currentTrack;
        private string queryInput;

        #endregion

        #region .ctr

        public MainWindowViewModel(IAuthManager authManager,
            ICredentialsReader credentialsReader,
            IServiceManager serviceManager,
            IMusicDownloadManager musicDownloadManager)
        {
            this.authManager = authManager;
            this.credentialsReader = credentialsReader;
            this.serviceManager = serviceManager;
            this.musicDownloadManager = musicDownloadManager;

            //  todo: handle it differently
            var musicService = this.serviceManager.GetServiceAsync("music").GetAwaiter().GetResult();
            if (musicService is IMusicService ms)
            {
                this.musicService = ms;
            }

            this.tracks = new ObservableCollection<Track>();
            this.queryInput = string.Empty;
        }

        #endregion

        #region Properties

        public ObservableCollection<Track> Tracks
        {
            get => this.tracks;
            set {
                this.tracks = value;
                Notify(nameof(Tracks));
            }
        }

        public string QueryInput
        {
            get => this.queryInput;
            set {
                this.queryInput = value;
                Notify(nameof(QueryInput));
            }
        }

        public Track CurrentTrack
        {
            get => this.currentTrack;
            set {
                this.currentTrack = value;
                Notify(nameof(CurrentTrack));
            }
        }

        #endregion

        #region Methods

        private void Notify(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void TriggerLogin(object o)
        {
            var response = await this.authManager.AuthenticateAsync((await this.credentialsReader.GetCredentialsAsync()));

            LoginReacted?.Invoke(this, new AuthEventArgs(response));
        }

        public async void TriggerDownload(object caller, bool overwrite)
        {
            var result = await this.musicDownloadManager.DownloadAsync(this.CurrentTrack, overwrite);

            DownloadReacted?.Invoke(this, new TrackDownloadEventArgs(result));
        }

        public async void TriggerQuery(object caller, string input)
        {

            var receivedTracks = await this.musicService.GetTracksAsync(string.Empty);
            this.Tracks = new ObservableCollection<Track>(receivedTracks);

            QueryReacted?.Invoke(this, EventArgs.Empty);
        }

        public void TriggerLogout(object caller)
        {
            Tracks.Clear();

            LogoutReacted?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
