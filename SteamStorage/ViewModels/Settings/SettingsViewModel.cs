using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Settings;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.ViewModels.Settings;

public class SettingsViewModel : ViewModelBase
{
    #region Fields

    private readonly SettingsModel _model;

    #endregion Fields

    #region Properties

    public IEnumerable<ThemeModel> ThemeModels
    {
        get => _model.ThemeModels;
    }

    public ThemeModel SelectedThemeModel
    {
        get => _model.SelectedThemeModel;
        set => _model.SelectedThemeModel = value;
    }

    #endregion Properties

    #region Commands

    public AsyncRelayCommand ExportToExcelCommand
    {
        get => _model.ExportToExcelCommand;
    }

    public RelayCommand OpenReferenceInformationCommand
    {
        get => _model.OpenReferenceInformationCommand;
    }

    #endregion Commands

    #region Constructor

    public SettingsViewModel(
        SettingsModel model)
    {
        _model = model;
        model.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
