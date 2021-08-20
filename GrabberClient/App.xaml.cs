﻿using GrabberClient.Auth;
using GrabberClient.Configuration;
using GrabberClient.Contracts;
using GrabberClient.Helpers;
using GrabberClient.Internals;
using GrabberClient.Services;
using GrabberClient.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows;
using VkNet;
using VkNet.AudioBypassService.Extensions;

namespace Grabber
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IConfiguration Configuration { get; private set; }
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(AppConstants.ConfigurationFileName, optional: false, reloadOnChange: true);

            Safeguard.TryRun(() =>
            {
                Configuration = builder.Build();

                var serviceCollection = new ServiceCollection();
                serviceCollection.AddAudioBypass();

                RegisterServices(serviceCollection);

                ServiceProvider = serviceCollection.BuildServiceProvider();

                var topWindow = ServiceProvider.GetService<IMainView>();

                topWindow.Show();
            }, terminateOnFailure: true);
        }

        private void RegisterServices(ServiceCollection services)
        {
            services.Configure<CredentialsSection>(Configuration.GetSection(AppConstants.ConfigurationCredentialsSectionName));
            services.Configure<DownloadSettingsSection>(Configuration.GetSection(AppConstants.ConfigurationDownloadSettingsSectionName));
            services.AddSingleton<IMainView, MainWindow>();
            services.AddTransient<IMainViewViewModel, MainWindowViewModel>();
            services.AddScoped<ICredentialsReader, ConfigurationCredentialsReader>();
            services.AddSingleton<VkApi>(f => new VkApi(services));
            services.AddSingleton<IAuthManager, VkAuthManager>();
            services.AddSingleton<IServiceManager, VkServiceManager>();
            services.AddScoped<IMusicService, VkMusicService>();
            services.AddScoped<IMusicDownloadService, VkMusicDownloadService>();
            services.AddScoped<IQueueService, DownloadQueueService>();
        }
    }
}