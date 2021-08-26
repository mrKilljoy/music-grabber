namespace GrabberClient.Contracts
{
    public interface IView
    {
        IViewModel ViewModel { get; }

        void Show();

        void Hide();
    }
}
