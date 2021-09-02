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
        public event EntityDownloadStartedEventHandler DownloadStarted;
        public event EntityDownloadCompletedEventHandler DownloadCompleted;
        public event EventHandler QueryCompleted;
        public event EventHandler LogoutCompleted;

        public event TrackQueuedEventHandler ElementEnqueued;
        public event TrackQueuedEventHandler ElementDequeued;

        #endregion

        #region Fields

        private readonly IAuthManager authManager;
        private readonly ICredentialsReader credentialsReader;
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
            IMusicService musicService,
            IMusicDownloadService musicDownloadManager,
            IQueueService queueManager)
        {
            this.authManager = authManager;
            this.credentialsReader = credentialsReader;
            this.musicService = musicService;
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

        public Task TriggerDownload(object caller, bool overwrite)
        {
            this.queueManager.Enqueue(this.currentTrack);
            return Task.CompletedTask;
        }

        public async Task TriggerQuery(object caller, string input)
        {
            var receivedTracks = await this.musicService.GetTracksAsync(input).ConfigureAwait(false);
            this.Tracks = new ObservableCollection<Track>(receivedTracks);

            this.QueryCompleted?.Invoke(this, EventArgs.Empty);
        }

        public Task TriggerLogout(object caller)
        {
            this.Tracks.Clear();

            this.LogoutCompleted?.Invoke(this, EventArgs.Empty);

            return Task.CompletedTask;
        }

        #endregion

        #region Private methods

        private async void TriggerEnqueueing(object caller, EntityQueuedEventArgs ea)
        {
            this.ElementEnqueued?.Invoke(this, ea);
            this.queueHandlingTimer.Start();
        }

        private async void TriggerDequeueing(object caller, EntityQueuedEventArgs ea)
        {
            this.ElementDequeued?.Invoke(this, ea);
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
                    var track = this.queueManager.Peek() as Track;
                    this.DownloadStarted?.Invoke(this, new EntityDownloadStartedEventArgs(track));
                    return await this.musicDownloadManager.DownloadAsync(track).ConfigureAwait(false);
                })
                .ContinueWith(taskResults =>
                {
                    var results = taskResults.Result;
                    var track = this.queueManager.Peek() as Track;

                    if (results.IsSuccess && results.OperationData.TryGet<Guid>(AppConstants.Metadata.UidField) == track.UID)
                    {
                        //  change it from UI context??
                        this.DownloadCompleted?.Invoke(this, new EntityDownloadCompletedEventArgs(results));
                        this.queueManager.Dequeue();
                    }

                    if (!results.IsSuccess)
                    {
                        this.queueManager.Dequeue();
                        var message = results.OperationData.TryGet<string>(AppConstants.Metadata.MessageField);
                        ErrorHelper.ShowError(message);
                        
                        DownloadCompleted?.Invoke(this, new EntityDownloadCompletedEventArgs(results));
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
            if (this.PropertyChanged is not null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
