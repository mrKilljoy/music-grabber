namespace GrabberClient.Contracts
{
    public interface IAuthView : IView
    {
        new IAuthViewViewModel ViewModel { get; }
    }
}
