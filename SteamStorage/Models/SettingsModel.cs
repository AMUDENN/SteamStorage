using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Services.ThemeService;
using SteamStorage.Utilities;
using SteamStorage.ViewModels;

namespace SteamStorage.Models;

public class SettingsModel : ModelBase
{
    #region Fields

    private readonly IThemeService _themeService;

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

    public SettingsModel(IThemeService themeService)
    {
        _themeService = themeService;

        ThemeModels =
        [
            new("Классическая", ThemeVariants.Classic),
            new("Лаймовая", ThemeVariants.Lime)
        ];

        ExportToExcelCommand = new(DoExportToExcelCommand);
        OpenReferenceInformationCommand = new(DoOpenReferenceInformationCommand);
    }

    #endregion Constructor

    #region Methods

    private void DoExportToExcelCommand()
    {

    }

    private void DoOpenReferenceInformationCommand()
    {

    }

    #endregion Methods
}
