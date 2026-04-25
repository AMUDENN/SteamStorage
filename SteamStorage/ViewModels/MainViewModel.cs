using SteamStorage.Models;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.ViewModels;

public class MainViewModel : ViewModelBase
{
    #region Fields

    private readonly MainModel _mainModel;

    #endregion Fields

    #region Properties

    public string? ImageUrl => _mainModel.ImageUrl;

    public string UserName => _mainModel.UserName;

    public string SteamId => _mainModel.SteamId;

    public bool IsUserLogin => _mainModel.IsUserLogin;

    public ViewModelBase CurrentViewModel => _mainModel.CurrentViewModel;

    public IEnumerable<NavigationModel> NavigationOptions => _mainModel.NavigationOptions;

    public NavigationModel? SelectedNavigationModel
    {
        get => _mainModel.SelectedNavigationModel;
        set => _mainModel.SelectedNavigationModel = value;
    }

    public bool IsSettingsChecked
    {
        get => _mainModel.IsSettingsChecked;
        set => _mainModel.IsSettingsChecked = value;
    }

    #endregion Properties

    #region Commands

    public RelayCommand LogInCommand => _mainModel.LogInCommand;

    public RelayCommand LogOutCommand => _mainModel.LogOutCommand;

    #endregion Commands

    #region Constructor

    public MainViewModel(
        MainModel mainModel)
    {
        _mainModel = mainModel;
        mainModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}