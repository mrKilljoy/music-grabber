namespace GrabberClient.Internals.Delegates
{
    public delegate void AuthEventHandler(object o, AuthEventArgs ea);
    public delegate void EntityDownloadCompletedEventHandler(object o, EntityDownloadCompletedEventArgs ea);
    public delegate void EntityDownloadStartedEventHandler(object o, EntityDownloadStartedEventArgs ea);
    public delegate void TrackQueuedEventHandler(object o, EntityQueuedEventArgs ea);
}
