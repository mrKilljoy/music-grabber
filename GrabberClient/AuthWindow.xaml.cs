using GrabberClient.Contracts;
using GrabberClient.Internals.Commands;
using GrabberClient.Internals.Delegates;
using GrabberClient.Models;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace GrabberClient
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window, IAuthView
    {
        #region Fields

        private readonly IAuthViewViewModel viewModel;

        private readonly IMainView mainView;

        #endregion

        #region Properties

        IViewModel IView.ViewModel => this.viewModel;

        public IAuthViewViewModel ViewModel => this.viewModel;

        #endregion

        #region .ctr

        public AuthWindow(IAuthViewViewModel viewModel, IMainView mainView)
        {
            InitializeComponent();

            this.viewModel = viewModel;
            this.mainView = mainView;

            this.SetBindings();
            this.SetHandlers();
        }

        #endregion

        #region Command handlers

        private async void HandleLoginCommandAsync(object o, ExecutedRoutedEventArgs ea)
        {
            await this.ViewModel.Authorize(new VkServiceCredentials()).ConfigureAwait(false);
        }

        private void HandleAppExitCommand(object o, ExecutedRoutedEventArgs ea)
        {
            Environment.Exit(0);
        }

        #endregion

        #region ViewModel event handlers

        private void HandleLogintEvent(object o, AuthEventArgs ea)
        {
            this.Dispatcher.BeginInvoke(delegate 
            {
                this.Hide();
                this.mainView.Show();
                
            }, DispatcherPriority.Normal);
        }

        private void HandleLogoutEvent(object o, EventArgs ea)
        {
            if (ea is CancelEventArgs cea)
                cea.Cancel = true;

            this.Dispatcher.BeginInvoke(delegate
            {
                this.mainView.Hide();
                this.Show();

            }, DispatcherPriority.Normal);
        }

        #endregion

        #region Other methods

        private void SetBindings()
        {
            this.CommandBindings.Add(new CommandBinding(AppCommands.ExitCommand, this.HandleAppExitCommand));
            this.CommandBindings.Add(new CommandBinding(AppCommands.LoginCommand, this.HandleLoginCommandAsync));
        }

        private void SetHandlers()
        {
            this.ViewModel.LoggedIn += this.HandleLogintEvent;
            this.mainView.ViewModel.LogoutCompleted += this.HandleLogoutEvent;
            this.mainView.Closing += this.HandleLogoutEvent;
        }

        #endregion
    }
}
