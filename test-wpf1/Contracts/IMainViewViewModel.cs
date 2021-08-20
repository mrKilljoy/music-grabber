using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using test_wpf1.Delegates;
using test_wpf1.Internals;
using test_wpf1.Models;

namespace test_wpf1.Contracts
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

        void TriggerLogin(object caller);

        void TriggerLogout(object caller);

        void TriggerDownload(object caller, bool overwrite = false);

        void TriggerQuery(object caller, string input);

        #endregion
    }
}