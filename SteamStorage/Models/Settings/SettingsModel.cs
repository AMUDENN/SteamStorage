﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.Services.NotificationService;
using SteamStorage.Services.Settings.SettingsService;
using SteamStorage.Services.ThemeService;
using SteamStorage.Utilities.Events.Settings;
using SteamStorage.Utilities.ThemeVariants;
using SteamStorage.Views.Windows;
using SteamStorageAPI.SDK;
using SteamStorageAPI.SDK.Services.ReferenceInformationService;
using SteamStorageAPI.SDK.Utilities;

namespace SteamStorage.Models.Settings;

public class SettingsModel : ModelBase
{
    #region Fields

    private readonly ApiClient _apiClient;
    private readonly MainWindow _mainWindow;
    private readonly IThemeService _themeService;
    private readonly ISettingsService _settingsService;
    private readonly INotificationService _notificationService;
    private readonly IReferenceInformationService _referenceInformationService;

    private ThemeModel _selectedThemeModel;

    #endregion Fields

    #region Properties

    public List<ThemeModel> ThemeModels { get; }

    public ThemeModel SelectedThemeModel
    {
        get => _selectedThemeModel;
        set
        {
            SetProperty(ref _selectedThemeModel, value);
            _themeService.ChangeTheme(value.ThemeVariant);
        }
    }

    #endregion Properties

    #region Commands

    public AsyncRelayCommand ExportToExcelCommand { get; }

    public RelayCommand OpenReferenceInformationCommand { get; }

    #endregion Commands

    #region Constructor

    public SettingsModel(
        ApiClient apiClient,
        MainWindow mainWindow,
        IThemeService themeService,
        ISettingsService settingsService,
        INotificationService notificationService,
        IReferenceInformationService referenceInformationService)
    {
        _apiClient = apiClient;
        _mainWindow = mainWindow;
        _themeService = themeService;
        _settingsService = settingsService;
        _notificationService = notificationService;
        _referenceInformationService = referenceInformationService;

        ThemeModels =
        [
            new("Классический", ThemeVariants.Classic),
            new("Лаймовый", ThemeVariants.Lime),
            new("Тёмный", ThemeVariants.VeryDark),
            new("Светлый", ThemeVariants.VeryLight)
        ];

        _selectedThemeModel = ThemeModels.First();

        SetTheme();

        settingsService.SettingsPropertyChanged += SettingsPropertyChangedHandler;

        ExportToExcelCommand = new(DoExportToExcelCommand);
        OpenReferenceInformationCommand = new(DoOpenReferenceInformationCommand);
    }

    #endregion Constructor

    #region Methods

    private void SettingsPropertyChangedHandler(object? sender, SettingsPropertyChangedEventArgs e)
    {
        if (e.Property != nameof(_settingsService.UserSettings.Theme)) return;
        SetTheme();
    }

    private void SetTheme()
    {
        SelectedThemeModel =
            ThemeModels.FirstOrDefault(
                x => x.ThemeVariant.ToString() == _settingsService.UserSettings.Theme?.ToString()) ??
            ThemeModels.First();
    }

    private async Task DoExportToExcelCommand(CancellationToken cancellationToken)
    {
        TopLevel? topLevel = TopLevel.GetTopLevel(_mainWindow);

        if (topLevel is null) return;

        IStorageFile? file = await topLevel.StorageProvider.SaveFilePickerAsync(new()
        {
            Title = "Сохранение файла",
            FileTypeChoices = new List<FilePickerFileType>
            {
                new("*.xlsx")
                {
                    Patterns = new[] { "*.xlsx" }
                }
            }
        });

        if (file is null) return;

        Stream? fileResult =
            await _apiClient.GetFileStreamAsync(ApiConstants.ApiMethods.GetExcelFile, cancellationToken);

        if (fileResult is null) return;

        await using Stream stream = await file.OpenWriteAsync();

        await fileResult.CopyToAsync(stream, cancellationToken);

        await _notificationService.ShowAsync("Файл сохранён",
            $"Файл {file.Name} успешно сохранён",
            cancellationToken: cancellationToken);
    }

    private void DoOpenReferenceInformationCommand()
    {
        _referenceInformationService.OpenReferenceInformation();
    }

    #endregion Methods
}
