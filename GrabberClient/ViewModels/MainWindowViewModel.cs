using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Timers;
using GrabberClient.Contracts;
using GrabberClient.Helpers;
using GrabberClient.Internals;
using GrabberClient.Internals.Delegates;
using GrabberClient.Models;

namespace GrabberClient.ViewModels
{
    public sealed class MainWindowViewModel : IMainViewViewModel
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        public event AuthEventHandler LoginReacted;
        public event TrackDownloadEventHandler DownloadReacted;
        public event EventHandler QueryReacted;
        public event EventHandler LogoutReacted;

        public event TrackQueuedEventHandler TrackEnqueueingReacted;
        public event TrackQueuedEventHandler TrackDequeueingReacted;

        #endregion

        #region Fields

        private readonly IAuthManager authManager;
        private readonly ICredentialsReader credentialsReader;
        private readonly IServiceManager serviceManager;
        private readonly IMusicDownloadService musicDownloadManager;
        private readonly IQueueService queueManager;

        private IMusicService musicService;

        private ObservableCollection<Track> tracks;
        private ObservableQueue<Track> queue;
        private Track currentTrack;
        private string queryInput;

        private readonly Timer queueHandlingTimer;

        private static readonly object syncObject = new object();
        private bool isBusy;

        #endregion

        #region .ctr

        public MainWindowViewModel(IAuthManager authManager,
            ICredentialsReader credentialsReader,
            IServiceManager serviceManager,
            IMusicDownloadService musicDownloadManager,
            IQueueService queueManager)
        {
            this.authManager = authManager;
            this.credentialsReader = credentialsReader;
            this.serviceManager = serviceManager;
            this.musicDownloadManager = musicDownloadManager;
            this.queueManager = queueManager;

            this.queueManager.QueueAdded += this.TriggerEnqueueing;
            this.queueManager.QueueRemoved += this.TriggerDequeueing;

            this.tracks = new ObservableCollection<Track>();
            this.queue = new ObservableQueue<Track>();
            this.queryInput = string.Empty;

            this.queueHandlingTimer = new Timer(500);
            this.queueHandlingTimer.Elapsed += QueueTimerTickAsync;
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

        public ObservableQueue<Track> Queue
        {
            get => this.queue;
            set
            {
                this.queue = value;
                Notify(nameof(Queue));
            }
        }

        #endregion

        #region Trigger methods for outside calls

        public async Task TriggerLogin(object o)
        {
            var response = await this.authManager.AuthenticateAsync(
                await this.credentialsReader.GetCredentialsAsync().ConfigureAwait(false)).ConfigureAwait(false);

            if (response.IsSuccess)
            {
                var musicService = await this.serviceManager.GetServiceAsync("music").ConfigureAwait(false);
                if (musicService is IMusicService ms)
                {
                    this.musicService = ms;
                }
            }

            LoginReacted?.Invoke(this, new AuthEventArgs(response));
        }

        public Task TriggerDownload(object caller, bool overwrite)
        {
            this.queueManager.Enqueue(this.currentTrack);
            return Task.CompletedTask;
        }

        public async Task TriggerQuery(object caller, string input)
        {
            var receivedTracks = await this.musicService.GetTracksAsync(input).ConfigureAwait(false);
            this.Tracks = new ObservableCollection<Track>(receivedTracks);

            QueryReacted?.Invoke(this, EventArgs.Empty);
        }

        public Task TriggerLogout(object caller)
        {
            Tracks.Clear();

            LogoutReacted?.Invoke(this, EventArgs.Empty);

            return Task.CompletedTask;
        }

        #endregion

        #region Private methods

        private async void TriggerEnqueueing(object caller, EntityQueuedEventArgs ea)
        {
            this.TrackEnqueueingReacted?.Invoke(this, ea);
            this.queueHandlingTimer.Start();
        }

        private async void TriggerDequeueing(object caller, EntityQueuedEventArgs ea)
        {
            this.TrackDequeueingReacted?.Invoke(this, ea);
        }

        private async void QueueTimerTickAsync(object o, EventArgs e)
        {
            if (this.queueManager.Count() == 0)
            {
                this.queueHandlingTimer.Stop();
                return;
            }

            if (!this.isBusy && this.queueManager.Count() > 0)
            {
                lock (syncObject)
                {
                    this.isBusy = true;
                }

                await Task.Run(async () =>
                {
                    var t = this.queueManager.Peek() as Track;
                    return await this.musicDownloadManager.DownloadAsync(t).ConfigureAwait(false);
                })
                .ContinueWith(a =>
                {
                    var results = a.Result;
                    var track = this.queueManager.Peek() as Track;

                    if (results.IsSuccess && results.OperationData.TryGet<Guid>("UID") == track.UID)
                    {
                        //  change it from UI context??
                        DownloadReacted?.Invoke(this, new TrackDownloadEventArgs(results));
                        this.queueManager.Dequeue();
                    }

                    if (!results.IsSuccess)
                    {
                        this.queueManager.Dequeue();
                        var message = results.OperationData.TryGet<string>("message");
                        ErrorHelper.ShowError(message);
                        
                        DownloadReacted?.Invoke(this, new TrackDownloadEventArgs(results));
                    }

                    lock(syncObject)
                    {
                        this.isBusy = false;
                    }
                }).ConfigureAwait(false);
            }
        }

        private void Notify(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
