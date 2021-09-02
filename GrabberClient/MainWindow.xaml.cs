using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using GrabberClient.Contracts;
using GrabberClient.Helpers;
using GrabberClient.Internals;
using GrabberClient.Internals.Commands;
using GrabberClient.Internals.Delegates;
using GrabberClient.Models;

namespace Grabber
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainView
    {
        #region Fields

        private DispatcherTimer queryDelayTimer;

        private readonly IMainViewViewModel viewModel;

        #endregion

        #region .ctr

        public MainWindow(IMainViewViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel = viewModel;
            this.DataContext = this.ViewModel;

            this.SetHandlers();
            this.SetBindings();
            this.SetTimer();
        }

        #endregion

        #region Properties

        public IMainViewViewModel ViewModel => this.viewModel;

        IViewModel IView.ViewModel => this.viewModel;

        #endregion

        #region Other methods

        private void SetHandlers()
        {
            this.tb1.TextChanged += this.HandleQueryInput;

            this.ViewModel.ElementEnqueued += this.HandleTrackEnqueueingEventAsync;
            this.ViewModel.ElementDequeued += this.HandleTrackDequeueingEventAsync;
            this.ViewModel.DownloadStarted += this.HandleTrackDownloadStartEventAsync;
            this.ViewModel.DownloadCompleted += this.HandleTrackDownloadEndEventAsync;
            this.ViewModel.QueryCompleted += this.HandleQueryEventAsync;
        }

        private void SetBindings()
        {
            this.CommandBindings.Add(new CommandBinding(AppCommands.LogoutCommand, this.HandleLogoutCommandAsync));
            this.CommandBindings.Add(new CommandBinding(AppCommands.DownloadCommand, this.HandleDownloadCommandAsync));
        }

        private void SetTimer()
        {
            this.queryDelayTimer = new DispatcherTimer(DispatcherPriority.Background, this.Dispatcher);
            this.queryDelayTimer.Interval = TimeSpan.FromMilliseconds(AppConstants.QueryDelayInMilliseconds);
            this.queryDelayTimer.Tick += this.HandleTimerTickAsync;
        }

        private void HandleQueryInput(object o, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.tb1.Text))
            {
                e.Handled = true;
                this.queryDelayTimer.Stop();
                this.Dispatcher.BeginInvoke(
                    delegate { this.tbStatus.Text = null; },
                    DispatcherPriority.Normal);

                return;
            }

            if (this.queryDelayTimer.IsEnabled)
            {
                this.queryDelayTimer.Stop();
                this.queryDelayTimer.Start();
            }
            else
                this.queryDelayTimer.Start();

            this.Dispatcher.BeginInvoke(
                delegate { this.tbStatus.Text = AppConstants.Statuses.PreparingTitle; },
                DispatcherPriority.Normal);
        }

        private async void HandleTimerTickAsync(object o, EventArgs e)
        {
            this.queryDelayTimer.Stop();
            await Task.Run(async () =>
            {
                await this.Dispatcher.BeginInvoke(delegate
                {
                    this.tbStatus.Text = AppConstants.Statuses.RetrievingDataTitle;
                    this.tb1.IsReadOnly = true;
                }, DispatcherPriority.Background);

                await this.ViewModel.TriggerQuery(this, this.ViewModel.QueryInput).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        #endregion

        #region Command handlers

        private async void HandleLogoutCommandAsync(object o, ExecutedRoutedEventArgs ea)
        {
            try
            {
                await this.ViewModel.TriggerLogout(this).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ErrorHelper.ShowError(string.Format(
                    AppConstants.Messages.UnknownErrorWithDetailsMessage,
                    ex.Message));
            }
        }

        private async void HandleDownloadCommandAsync(object o, ExecutedRoutedEventArgs ea)
        {
            try
            {
                if (this.ViewModel.CurrentTrack is null)
                {
                    ErrorHelper.ShowError(AppConstants.Messages.NoTrackSelectedMessage);
                    return;
                }

                await this.ViewModel.TriggerDownload(this).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ErrorHelper.ShowError(string.Format(
                    AppConstants.Messages.UnknownErrorWithDetailsMessage,
                    ex.Message));
            }
        }

        #endregion

        #region Event handlers

        private async void HandleQueryEventAsync(object sender, EventArgs e)
        {
            await this.Dispatcher.BeginInvoke(delegate
            {
                this.tbStatus.Text = null;
                this.tb1.IsReadOnly = false;
            }, DispatcherPriority.Normal);
        }

        private async void HandleTrackEnqueueingEventAsync(object sender, EntityQueuedEventArgs e)
        {
            await this.Dispatcher.BeginInvoke(delegate
            {
                this.ViewModel.Queue.Enqueue(e.Track as Track);
            }, DispatcherPriority.Normal);
        }

        private async void HandleTrackDequeueingEventAsync(object sender, EntityQueuedEventArgs e)
        {
            await this.Dispatcher.BeginInvoke(delegate
            {
                this.ViewModel.Queue.Dequeue();
                this.tbStatus.Text = null;
            }, DispatcherPriority.Normal);
        }

        private async void HandleTrackDownloadStartEventAsync(object sender, EntityDownloadStartedEventArgs e)
        {
            await this.Dispatcher.BeginInvoke(delegate
            {
                var track = e.Entity as Track;
                this.tbStatus.Text = $"...downloading \"{track.Title} - {track.Artist}\"";
            }, DispatcherPriority.Background);
        }

        private async void HandleTrackDownloadEndEventAsync(object sender, EntityDownloadCompletedEventArgs e)
        {
            await this.Dispatcher.BeginInvoke(delegate
            {
                this.tbStatus.Text = null;
            }, DispatcherPriority.Background);
        }

        #endregion

        private async void xpndr_Expanded(object sender, RoutedEventArgs e)
        {
            await this.Dispatcher.BeginInvoke(delegate
            {
                if (this.xpndr.IsExpanded)
                {
                    Canvas.SetZIndex(this.cnvQueueLayer, 5);
                    Canvas.SetZIndex(this.xpndr, 5);
                }
            }, DispatcherPriority.Normal);
        }

        private async void xpndr_Collapsed(object sender, RoutedEventArgs e)
        {
            await this.Dispatcher.BeginInvoke(delegate
            {
                if (!this.xpndr.IsExpanded)
                {
                    Canvas.SetZIndex(this.cnvQueueLayer, -1);
                    Canvas.SetZIndex(this.xpndr, -1);
                }
            }, DispatcherPriority.Normal);
        }
    }
}
