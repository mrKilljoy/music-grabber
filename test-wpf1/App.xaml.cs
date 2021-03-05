using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows;
using test_wpf1.Auth;
using test_wpf1.Configuration;
using test_wpf1.Contracts;
using test_wpf1.Helpers;
using test_wpf1.Services;
using test_wpf1.ViewModels;

namespace test_wpf1
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

            try
            {
                Configuration = builder.Build();

                var serviceCollection = new ServiceCollection();
                RegisterServices(serviceCollection);

                ServiceProvider = serviceCollection.BuildServiceProvider();

                var topWindow = ServiceProvider.GetService<IMainView>();
                topWindow.Show();
            }
            catch (FileNotFoundException)
            {
                ErrorHelper.ShowError(AppConstants.Messages.MissingConfigurationFileErrorMessage);
                Environment.Exit(1);
            }
            catch (Exception)
            {
                ErrorHelper.ShowError(AppConstants.Messages.UnknownErrorMessage);
                Environment.Exit(1);
            }
        }

        private void RegisterServices(ServiceCollection services)
        {
            services.Configure<CredentialsSection>(Configuration.GetSection(AppConstants.ConfigurationCredentialsSectionName));
            services.Configure<DownloadSettingsSection>(Configuration.GetSection(AppConstants.ConfigurationDownloadSettingsSectionName));
            services.AddSingleton<IMainView, MainWindow>();
            services.AddTransient<IMainViewViewModel, MainWindowViewModel>();
            services.AddScoped<ICredentialsReader, ConfigurationCredentialsReader>();
            services.AddScoped<IAuthManager, DummyAuthManager>();
            services.AddScoped<IServiceManager, DummyServiceManager>();
            services.AddScoped<IMusicDownloadManager, DummyMusicDownloadManager>();
        }
    }
}
