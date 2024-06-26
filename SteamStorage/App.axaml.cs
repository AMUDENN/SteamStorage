using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using SteamStorage.ViewModels;
using System;
using Avalonia.Threading;
using SteamStorage.Models;
using SteamStorage.Models.Actives;
using SteamStorage.Models.Archives;
using SteamStorage.Models.Dialog;
using SteamStorage.Models.Home;
using SteamStorage.Models.Inventory;
using SteamStorage.Models.Profile;
using SteamStorage.Models.Settings;
using SteamStorage.Models.Windows;
using SteamStorage.Services.ClipboardService;
using SteamStorage.Services.DialogService;
using SteamStorage.Services.NotificationService;
using SteamStorage.Services.Settings.SettingsService;
using SteamStorage.Services.ThemeService;
using SteamStorage.Utilities;
using SteamStorage.ViewModels.Actives;
using SteamStorage.ViewModels.Archives;
using SteamStorage.ViewModels.Dialog;
using SteamStorage.ViewModels.Home;
using SteamStorage.ViewModels.Inventory;
using SteamStorage.ViewModels.Profile;
using SteamStorage.ViewModels.Settings;
using SteamStorage.ViewModels.Windows;
using SteamStorage.Views.Windows;
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
            Dispatcher.UIThread.UnhandledException += UnhandledExceptionHandler;
            Container = GetServiceCollection().BuildServiceProvider();
        }

        #endregion Constructor

        #region Methods
        
        private static void UnhandledExceptionHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.Exception);
            Container.GetService<INotificationService>()?
                .ShowAsync("Ошибка во время выполнения", e.Exception.Message);
            e.Handled = true;
        }

        private static ServiceCollection GetServiceCollection()
        {
            ServiceCollection services = [];

            
            //SteamStorageApi
            services.AddSteamStorageApi(options =>
            {
                options.ClientTimeout = ProgramConstants.API_CLIENT_TIMEOUT;
            });

            
            //SteamStorageApi Services
            services.AddSteamStorageAuthorizationService();
            services.AddSteamStoragePingService();
            services.AddSteamStorageReferenceInformationService();
            services.AddSteamStorageLoggerService(options =>
            {
                options.ProgramName = ProgramConstants.LOG_PROGRAM_NAME;
                options.DateFormat = ProgramConstants.LOG_DATE_FORMAT;
                options.DateTimeFormat = ProgramConstants.LOG_DATETIME_FORMAT;
                options.LogFilesLifetime = ProgramConstants.LOG_FILES_LIFETIME_DAYS;
            });


            //Custom Services
            services.AddScoped<IThemeService, ThemeService>();
            services.AddScoped<IClipboardService, ClipboardService>();
            services.AddScoped<IDialogService, DialogService>();
            services.AddSingleton<INotificationService, NotificationService>();
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
            services.AddSingleton<ListActivesModel>();
            
            services.AddSingleton<ArchiveGroupEditModel>();
            services.AddSingleton<ArchiveGroupsModel>();
            services.AddSingleton<ArchiveEditModel>();
            services.AddSingleton<ArchivesModel>();
            services.AddSingleton<ArchivesReviewModel>();
            services.AddSingleton<ListArchivesModel>();
            
            services.AddSingleton<MessageDialogModel>();
            services.AddSingleton<TextConfirmDialogModel>();
            
            services.AddSingleton<HomeModel>();
            services.AddSingleton<ListItemsModel>();
            services.AddSingleton<StatisticsModel>();
            
            services.AddSingleton<InventoryModel>();
            
            services.AddSingleton<ProfileModel>();
            
            services.AddSingleton<SettingsModel>();
            
            services.AddSingleton<ChartTooltipModel>();
            services.AddSingleton<CurrenciesModel>();
            services.AddSingleton<GamesModel>();
            services.AddSingleton<PagesModel>();
            services.AddSingleton<PeriodsModel>();
            services.AddSingleton<UserModel>();
            
            
            //ViewModels
            services.AddSingleton<MainViewModel>();

            services.AddSingleton<ActiveEditViewModel>();
            services.AddSingleton<ActiveGroupEditViewModel>();
            services.AddSingleton<ActiveSoldViewModel>();
            services.AddSingleton<ActivesReviewViewModel>();
            services.AddSingleton<ActivesViewModel>();
            services.AddSingleton<ListActivesViewModel>();
            
            services.AddSingleton<ArchiveEditViewModel>();
            services.AddSingleton<ArchiveGroupEditViewModel>();
            services.AddSingleton<ArchivesReviewViewModel>();
            services.AddSingleton<ArchivesViewModel>();
            services.AddSingleton<ListArchivesViewModel>();
            
            services.AddSingleton<MessageDialogViewModel>();
            services.AddSingleton<TextConfirmDialogViewModel>();
            
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<ListItemsViewModel>();
            services.AddSingleton<StatisticsViewModel>();
            
            services.AddSingleton<InventoryViewModel>();
            
            services.AddSingleton<ProfileViewModel>();
            
            services.AddSingleton<SettingsViewModel>();
            
            services.AddSingleton<DefaultViewModel>();
            

            return services;
        }
        
        public static T? GetService<T>()
        {
            return Container.GetService<T>();
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
