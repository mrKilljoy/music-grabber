using System.Windows.Input;

namespace test_wpf1.Internals.Commands
{
    public static class AppCommands
    {
        public static readonly ICommand LoginCommand = new RoutedUICommand("Login", nameof(LoginCommand), typeof(AppCommands));

        public static readonly ICommand LogoutCommand = new RoutedUICommand("Logout", nameof(LogoutCommand), typeof(AppCommands));

        public static readonly ICommand DownloadCommand = new RoutedUICommand("Download", nameof(DownloadCommand), typeof(AppCommands));
    }
}
