using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Services.Settings.SettingsService;
using SteamStorage.Services.ThemeService;
using SteamStorage.Utilities.Events;
using SteamStorage.Utilities.ThemeVariants;

namespace SteamStorage.Models;

public class SettingsModel : ModelBase
{
    #region Fields

    private readonly IThemeService _themeService;
    private readonly ISettingsService _settingsService;

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

    public RelayCommand ExportToExcelCommand { get; }

    public RelayCommand OpenReferenceInformationCommand { get; }

    #endregion Commands

    #region Constructor

    public SettingsModel(IThemeService themeService, ISettingsService settingsService)
    {
        _themeService = themeService;
        _settingsService = settingsService;

        ThemeModels =
        [
            new("Классический", ThemeVariants.Classic),
            new("Лаймовый", ThemeVariants.Lime)
        ];

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

    private void DoExportToExcelCommand()
    {

    }

    private void DoOpenReferenceInformationCommand()
    {

    }

    #endregion Methods
}
