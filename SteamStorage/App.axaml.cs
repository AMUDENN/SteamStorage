using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using SteamStorage.ViewModels;
using SteamStorage.Views;
using System;
using SteamStorage.Models;
using SteamStorage.Services.DialogService;
using SteamStorage.Services.Settings.SettingsService;
using SteamStorage.Services.ThemeService;
using SteamStorage.Utilities;
using SteamStorageAPI.SDK;
using SteamStorageAPI.SDK.Utilities.Extensions.ServiceCollection;

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
            Container = GetServiceCollection().BuildServiceProvider();
        }

        #endregion Constructor

        #region Methods

        private static ServiceCollection GetServiceCollection()
        {
            ServiceCollection services = [];

            //SteamStorageApi
            services.AddSteamStorageApi(options =>
            {
                options.ClientTimeout = ProgramConstants.API_CLIENT_TIMEOUT;
            });

            //Custom SteamStorageApi Services
            services.AddSteamStorageAuthorizationService();
            services.AddSteamStoragePingService();
            services.AddSteamStorageLoggerService(options =>
            {
                options.ProgramName = ProgramConstants.LOG_PROGRAM_NAME;
                options.DateFormat = ProgramConstants.LOG_DATE_FORMAT;
                options.DateTimeFormat = ProgramConstants.LOG_DATETIME_FORMAT;
                options.LogFilesLifetime = ProgramConstants.LOG_FILES_LIFETIME_DAYS;
            });


            //Custom Services
            services.AddScoped<IThemeService, ThemeService>();
            services.AddScoped<IDialogService, DialogService>();
            services.AddSingleton<ISettingsService, SettingsService>(x =>
                new(ProgramConstants.PROGRAM_NAME,
                    x.GetRequiredService<ApiClient>(),
                    x.GetRequiredService<IThemeService>()));


            //MainWindow
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowModel>();
            services.AddSingleton<MainWindowViewModel>();


            //DialogWindow
            services.AddSingleton<DialogWindowModel>();
            services.AddSingleton<DialogWindowViewModel>();


            //Models
            services.AddSingleton<MainModel>();

            services.AddSingleton<ActiveEditModel>();
            services.AddSingleton<ActiveGroupEditModel>();
            services.AddSingleton<ActiveGroupsModel>();
            services.AddSingleton<ActivesModel>();
            services.AddSingleton<ActiveSoldModel>();
            services.AddSingleton<ActivesReviewModel>();
            services.AddSingleton<ArchiveGroupEditModel>();
            services.AddSingleton<ArchiveGroupsModel>();
            services.AddSingleton<ArchiveEditModel>();
            services.AddSingleton<ArchivesModel>();
            services.AddSingleton<ArchivesReviewModel>();
            services.AddSingleton<ChartTooltipModel>();
            services.AddSingleton<CurrenciesModel>();
            services.AddSingleton<GamesModel>();
            services.AddSingleton<HomeModel>();
            services.AddSingleton<InventoryModel>();
            services.AddSingleton<ListActivesModel>();
            services.AddSingleton<ListArchivesModel>();
            services.AddSingleton<ListItemsModel>();
            services.AddSingleton<MessageDialogModel>();
            services.AddSingleton<PagesModel>();
            services.AddSingleton<ProfileModel>();
            services.AddSingleton<SettingsModel>();
            services.AddSingleton<StatisticsModel>();
            services.AddSingleton<TextConfirmDialogModel>();
            services.AddSingleton<UserModel>();
            
            
            //ViewModels
            services.AddSingleton<MainViewModel>();

            services.AddSingleton<ActiveEditViewModel>();
            services.AddSingleton<ActiveGroupEditViewModel>();
            services.AddSingleton<ActiveSoldViewModel>();
            services.AddSingleton<ActivesReviewViewModel>();
            services.AddSingleton<ActivesViewModel>();
            services.AddSingleton<ArchiveEditViewModel>();
            services.AddSingleton<ArchiveGroupEditViewModel>();
            services.AddSingleton<ArchivesReviewViewModel>();
            services.AddSingleton<ArchivesViewModel>();
            services.AddSingleton<DefaultViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<InventoryViewModel>();
            services.AddSingleton<ListActivesViewModel>();
            services.AddSingleton<ListArchivesViewModel>();
            services.AddSingleton<ListItemsViewModel>();
            services.AddSingleton<MessageDialogViewModel>();
            services.AddSingleton<ProfileViewModel>();
            services.AddSingleton<SettingsViewModel>();
            services.AddSingleton<StatisticsViewModel>();
            services.AddSingleton<TextConfirmDialogViewModel>();

            return services;
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

        #endregion Methods
    }
}
