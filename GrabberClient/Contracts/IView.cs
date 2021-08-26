using System.ComponentModel;

namespace GrabberClient.Contracts
{
    public interface IView
    {
        public event CancelEventHandler Closing;

        IViewModel ViewModel { get; }

        void Show();

        void Hide();
    }
}
