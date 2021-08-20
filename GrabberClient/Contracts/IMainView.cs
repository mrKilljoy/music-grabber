namespace GrabberClient.Contracts
{
    public interface IMainView : IView
    {
        new IMainViewViewModel ViewModel { get; }
    }
}
