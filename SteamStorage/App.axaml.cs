using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using SteamStorage.ViewModels;
using SteamStorage.Views;
using System;
using SteamStorage.Models;
using SteamStorageAPI;
using SteamStorageAPI.Utilities;

namespace SteamStorage
{
    public partial class App : Application
    {
        private static IServiceProvider Container { get; set; }

        static App()
        {
            ServiceCollection services = [];

            //ApiClient
            services.AddHttpClient(ApiClient.CLIENT_NAME, client =>
            {
                client.Timeout = TimeSpan.FromSeconds(2);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            }).AddHttpMessageHandler<TokenHandler>();
            services.AddSingleton<TokenHandler>();
            services.AddSingleton<ApiClient>();

            //MainWindow
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();

            //ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<ActivesViewModel>();
            services.AddSingleton<ArchiveViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<InventoryViewModel>();
            services.AddSingleton<ProfileViewModel>();
            services.AddSingleton<SettingsViewModel>();
            
            //Models
            services.AddSingleton<UserModel>();
            services.AddSingleton<MainModel>();

            Container = services.BuildServiceProvider();
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                BindingPlugins.DataValidators.RemoveAt(0);

                desktop.MainWindow = Container.GetService<MainWindow>();

                if (desktop.MainWindow is null)
                    throw new("something went wrong during initializing DI container. MainWindow is missing");

                desktop.MainWindow.DataContext = Container.GetService<MainWindowViewModel>();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}