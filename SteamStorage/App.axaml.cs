using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using SteamStorage.ViewModels;
using SteamStorage.Views;
using System;
using SteamStorage.Models;
using SteamStorage.Services.Settings.SettingsService;
using SteamStorage.Services.ThemeService;
using SteamStorage.Utilities;
using SteamStorageAPI;
using SteamStorageAPI.Services.AuthorizationService;
using SteamStorageAPI.Services.Logger.LoggerService;
using SteamStorageAPI.Services.Ping.PingService;
using SteamStorageAPI.Utilities;

namespace SteamStorage
{
    public partial class App : Application
    {
        #region Properties

        private static IServiceProvider Container { get; }

        #endregion Properties

        #region Constructor

        static App()
        {
            ServiceCollection services = [];

            //ApiClient
            services.AddHttpClient(ApiConstants.CLIENT_NAME, client =>
            {
                client.Timeout = TimeSpan.FromSeconds(15);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            }).AddHttpMessageHandler<TokenHandler>().AddHttpMessageHandler<UnauthorizedHandler>();
            services.AddScoped<TokenHandler>();
            services.AddScoped<UnauthorizedHandler>();
            services.AddSingleton<ApiClient>();

            //Custom API Services
            services.AddSingleton<IAuthorizationService, AuthorizationService>();
            services.AddSingleton<ILoggerService, LoggerService>(_ => new(ApiConstants.LOG_PROGRAM_NAME,
                ApiConstants.LOG_DATE_FORMAT, ApiConstants.LOG_DATETIME_FORMAT));
            services.AddSingleton<IPingService, PingService>();

            //Custom Services
            services.AddScoped<IThemeService, ThemeService>();
            services.AddSingleton<ISettingsService, SettingsService>(x => new(ProgramConstants.PROGRAM_NAME,
                x.GetRequiredService<ApiClient>(), x.GetRequiredService<IThemeService>()));

            //MainWindow
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowModel>();
            services.AddSingleton<MainWindowViewModel>();

            //ViewModels
            services.AddSingleton<MainViewModel>();

            services.AddSingleton<ActivesReviewViewModel>();
            services.AddSingleton<ActivesViewModel>();
            services.AddSingleton<ArchiveViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<InventoryViewModel>();
            services.AddSingleton<ListActivesViewModel>();
            services.AddSingleton<ListItemsViewModel>();
            services.AddSingleton<ProfileViewModel>();
            services.AddSingleton<SettingsViewModel>();
            services.AddSingleton<StatisticsViewModel>();

            //Models
            services.AddSingleton<MainModel>();

            services.AddSingleton<ActiveGroupsModel>();
            services.AddSingleton<ActivesModel>();
            services.AddSingleton<ActivesReviewModel>();
            services.AddSingleton<ChartTooltipModel>();
            services.AddSingleton<GamesModel>();
            services.AddSingleton<HomeModel>();
            services.AddSingleton<InventoryModel>();
            services.AddSingleton<ListActivesModel>();
            services.AddSingleton<ListItemsModel>();
            services.AddSingleton<SettingsModel>();
            services.AddSingleton<StatisticsModel>();
            services.AddSingleton<UserModel>();


            Container = services.BuildServiceProvider();
        }

        #endregion Constructor

        #region Methods

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

        #endregion Methods
    }
}
