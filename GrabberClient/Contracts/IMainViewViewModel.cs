using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GrabberClient.Internals;
using GrabberClient.Internals.Delegates;
using GrabberClient.Models;

namespace GrabberClient.Contracts
{
    public interface IMainViewViewModel : IViewModel
    {
        #region Events

        event EventHandler LogoutCompleted;
        event EntityDownloadStartedEventHandler DownloadStarted;
        event EntityDownloadCompletedEventHandler DownloadCompleted;
        event EventHandler QueryCompleted;
        event TrackQueuedEventHandler ElementEnqueued;
        event TrackQueuedEventHandler ElementDequeued;

        #endregion

        #region Properties

        ObservableCollection<Track> Tracks { get; set; }

        Track CurrentTrack { get; set; }

        string QueryInput { get; set; }

        ObservableQueue<Track> Queue { get; set; }

        #endregion

        #region Methods

        Task TriggerLogout(object caller);

        Task TriggerDownload(object caller, bool overwrite = false);

        Task TriggerQuery(object caller, string input);

        #endregion
    }
}