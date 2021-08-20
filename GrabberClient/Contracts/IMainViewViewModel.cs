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

        event AuthEventHandler LoginReacted;
        event EventHandler LogoutReacted;
        event TrackDownloadEventHandler DownloadReacted;
        event EventHandler QueryReacted;
        event TrackQueuedEventHandler TrackEnqueueingReacted;
        event TrackQueuedEventHandler TrackDequeueingReacted;

        #endregion

        #region Properties

        ObservableCollection<Track> Tracks { get; set; }

        Track CurrentTrack { get; set; }

        string QueryInput { get; set; }

        ObservableQueue<Track> Queue { get; set; }

        #endregion

        #region Methods

        Task TriggerLogin(object caller);

        Task TriggerLogout(object caller);

        Task TriggerDownload(object caller, bool overwrite = false);

        Task TriggerQuery(object caller, string input);

        #endregion
    }
}