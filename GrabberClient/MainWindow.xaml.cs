﻿using System;
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

            //  prepare the app's initial state
            //this.SetControlsState(false);
            //this.btnLogin.IsEnabled = true;

            //  todo: remove button
            this.btnLogin.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Properties

        public IMainViewViewModel ViewModel => this.viewModel;

        IViewModel IView.ViewModel => this.viewModel;

        #endregion

        #region Other methods

        private void SetHandlers()
        {
            this.tb1.TextChanged += HandleQueryInput;

            //this.ViewModel.LoginReacted += HandleLoginEvent;
            //this.ViewModel.LogoutReacted += HandleLogoutEvent;
            this.ViewModel.TrackEnqueueingReacted += HandleTrackEnqueueingEvent;
            this.ViewModel.TrackDequeueingReacted += HandleTrackDequeueingEvent;
            this.ViewModel.QueryReacted += HandleQueryEvent;
        }

        private void SetBindings()
        {
            //this.CommandBindings.Add(new CommandBinding(AppCommands.LoginCommand, HandleLoginCommand));
            this.CommandBindings.Add(new CommandBinding(AppCommands.LogoutCommand, HandleLogoutCommand));
            this.CommandBindings.Add(new CommandBinding(AppCommands.DownloadCommand, HandleDownloadCommand));
        }

        private void SetControlsState(bool isEnabled)
        {
            foreach (var c in this.innerGrid.Children)
            {
                if (c is Control ctrl)
                    ctrl.IsEnabled = isEnabled;
            }

            this.xpndr.IsEnabled = isEnabled;
            this.xpndr.IsExpanded = false;
        } 

        private void SetTimer()
        {
            this.queryDelayTimer = new DispatcherTimer(DispatcherPriority.Background, this.Dispatcher);
            this.queryDelayTimer.Interval = TimeSpan.FromMilliseconds(AppConstants.QueryDelayInMilliseconds);
            this.queryDelayTimer.Tick += this.HandleTimerTickAsync;
        }
        
        private void SetAsLoggedOut()
        {
            this.SetControlsState(false);

            this.btnLogin.IsEnabled = true;
            this.xpndr.IsExpanded = false;
            this.xpndr.IsEnabled = false;
            this.tb1.Text = null;
            this.tbStatus.Text = null;
        }

        private void HandleQueryInput(object o, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.tb1.Text))
            {
                e.Handled = true;
                this.queryDelayTimer.Stop();
                this.Dispatcher.BeginInvoke(delegate { this.tbStatus.Text = null; }, DispatcherPriority.Normal);
                return;
            }

            if (this.queryDelayTimer.IsEnabled)
            {
                this.queryDelayTimer.Stop();
                this.queryDelayTimer.Start();
            }
            else
                this.queryDelayTimer.Start();

            this.Dispatcher.BeginInvoke(delegate { this.tbStatus.Text = AppConstants.Statuses.PreparingTitle; }, 
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

        private async void HandleLoginCommand(object o, ExecutedRoutedEventArgs ea)
        {
            try
            {
                await this.ViewModel.TriggerLogin(this).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ErrorHelper.ShowError(string.Format(AppConstants.Messages.UnknownErrorWithDetailsMessage, ex.Message));
            }
        }

        private async void HandleLogoutCommand(object o, ExecutedRoutedEventArgs ea)
        {
            try
            {
                await this.ViewModel.TriggerLogout(this).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ErrorHelper.ShowError(string.Format(AppConstants.Messages.UnknownErrorWithDetailsMessage, ex.Message));
            }
        }

        private async void HandleDownloadCommand(object o, ExecutedRoutedEventArgs ea)
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
                ErrorHelper.ShowError(string.Format(AppConstants.Messages.UnknownErrorWithDetailsMessage, ex.Message));
            }
        }

        #endregion

        #region Event handlers

        private async void HandleLoginEvent(object o, AuthEventArgs ea)
        {
            if (ea.AuthResults != null && ea.AuthResults.IsSuccess)
            {
                await Task.Run(async () =>
                {
                    await this.Dispatcher.BeginInvoke(delegate { this.tbStatus.Text = AppConstants.Statuses.AuthenticationTitle; },
                        DispatcherPriority.Normal);

                    //  emulate auth process
                    await Task.Delay(1000);

                    await this.Dispatcher.BeginInvoke(delegate
                    {
                        SetControlsState(true);

                        this.btnLogin.IsEnabled = false;
                        this.tbStatus.Text = null;
                    }, DispatcherPriority.Normal);
                });
            }
        }

        private async void HandleLogoutEvent(object o, EventArgs ea)
        {
            await this.Dispatcher.BeginInvoke(delegate { SetAsLoggedOut(); }, DispatcherPriority.Normal);
        }

        private async void HandleQueryEvent(object sender, EventArgs e)
        {
            await this.Dispatcher.BeginInvoke(delegate
            {
                this.tbStatus.Text = null;
                this.tb1.IsReadOnly = false;
            }, DispatcherPriority.Normal);
        }

        private async void HandleTrackEnqueueingEvent(object sender, EntityQueuedEventArgs e)
        {
            await this.Dispatcher.BeginInvoke(delegate
            {
                this.ViewModel.Queue.Enqueue(e.Track as Track);
            }, DispatcherPriority.Normal);
        }

        private async void HandleTrackDequeueingEvent(object sender, EntityQueuedEventArgs e)
        {
            await this.Dispatcher.BeginInvoke(delegate
            {
                this.ViewModel.Queue.Dequeue();
            }, DispatcherPriority.Normal);
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
