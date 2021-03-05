namespace test_wpf1.Contracts
{
    public interface IView
    {
        IViewModel ViewModel { get; }

        void Show();
    }
}
