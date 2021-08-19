using System;

namespace test_wpf1.Delegates
{
    public delegate void AuthEventHandler(object o, AuthEventArgs ea);
    public delegate void TrackDownloadEventHandler(object o, TrackDownloadEventArgs ea);
    public delegate void TrackQueuedEventHandler(object o, EntityQueuedEventArgs ea);
}
