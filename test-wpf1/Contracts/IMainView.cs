namespace test_wpf1.Contracts
{
    public interface IMainView : IView
    {
        new IMainViewViewModel ViewModel { get; }
    }
}
