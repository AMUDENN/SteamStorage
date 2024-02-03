using SteamStorage.Models;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.UtilityModels;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.ViewModels;

public class MainViewModel : ViewModelBase
{
    #region Fields

    private readonly MainModel _model;

    #endregion Fields

    #region Properties

    public string? ImageUrl
    {
        get => _model.ImageUrl;
    }

    public string UserName
    {
        get => _model.UserName;
    }

    public string SteamId
    {
        get => _model.SteamId;
    }

    public bool IsUserLogin
    {
        get => _model.IsUserLogin;
    }

    public ViewModelBase CurrentViewModel
    {
        get => _model.CurrentViewModel;
    }

    public IEnumerable<NavigationModel> NavigationOptions
    {
        get => _model.NavigationOptions;
    }

    public NavigationModel? SelectedNavigationModel
    {
        get => _model.SelectedNavigationModel;
        set => _model.SelectedNavigationModel = value;
    }

    public bool IsSettingsChecked
    {
        get => _model.IsSettingsChecked;
        set => _model.IsSettingsChecked = value;
    }

    #endregion Properties

    #region Commands

    public RelayCommand LogInCommand
    {
        get => _model.LogInCommand;
    }

    public RelayCommand LogOutCommand
    {
        get => _model.LogOutCommand;
    }

    #endregion Commands

    #region Constructor

    public MainViewModel(MainModel model)
    {
        _model = model;
        _model.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
