using System.Windows;

namespace GrabberClient.Helpers
{
    public static class ErrorHelper
    {
        public static MessageBoxResult ShowError(string message)
        {
            return MessageBox.Show(
                messageBoxText: message,
                caption: nameof(MessageBoxImage.Error),
                button: MessageBoxButton.OK,
                icon: MessageBoxImage.Error);
        }
    }
}
