using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.ViewModels;
using SteamStorageAPI;

namespace SteamStorage.Models;

public class MainModel : ObservableObject
{
    #region Constants

    private const string USERNAME = "Username";
    private const string STEAM_ID = "SteamID";

    #endregion Constants

    #region Fields

    private readonly ApiClient _apiClient;
    private readonly UserModel _userModel;

    private string? _imageUrl;
    private string _userName;
    private string _steamId;
    private bool _isUserLogin;

    private ViewModelBase _currentViewModel;
    private readonly ViewModelBase _settingsViewModel;

    private readonly IEnumerable<NavigationModel> _navigationOptions;
    private NavigationModel? _selectedNavigationModel;
    private bool _isSettingsChecked;

    private readonly RelayCommand _logInCommand;
    private readonly RelayCommand _logOutCommand;

    #endregion Fields

    #region Properties

    public string? ImageUrl
    {
        get => _imageUrl;
        set => SetProperty(ref _imageUrl, value);
    }

    public string UserName
    {
        get => _userName;
        set => SetProperty(ref _userName, value);
    }

    public string SteamId
    {
        get => _steamId;
        set => SetProperty(ref _steamId, value);
    }

    public bool IsUserLogin
    {
        get => _isUserLogin;
        set => SetProperty(ref _isUserLogin, value);
    }

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set => SetProperty(ref _currentViewModel, value);
    }

    public IEnumerable<NavigationModel> NavigationOptions
    {
        get => _navigationOptions;
    }

    public NavigationModel? SelectedNavigationModel
    {
        get => _selectedNavigationModel;
        set
        {
            SetProperty(ref _selectedNavigationModel, value);
            if (value is null) return;
            CurrentViewModel = value.Page;
            IsSettingsChecked = false;
        }
    }

    public bool IsSettingsChecked
    {
        get => _isSettingsChecked;
        set
        {
            SetProperty(ref _isSettingsChecked, value);
            if (!value) return;
            SelectedNavigationModel = null;
            CurrentViewModel = _settingsViewModel;
        }
    }

    #endregion Properties

    #region Commands

    public RelayCommand LogInCommand
    {
        get => _logInCommand;
    }

    public RelayCommand LogOutCommand
    {
        get => _logOutCommand;
    }

    #endregion Commands

    #region Constructor

    public MainModel(ApiClient apiClient,
        UserModel userModel,
        ActivesViewModel activesViewModel,
        ArchiveViewModel archiveViewModel,
        HomeViewModel homeViewModel,
        InventoryViewModel inventoryViewModel,
        ProfileViewModel profileViewModel,
        SettingsViewModel settingsViewModel)
    {
        _apiClient = apiClient;
        _userModel = userModel;

        _navigationOptions =
        [
            new("HomeImage", "Главная", homeViewModel),
            new("ActivesImage", "Активы", activesViewModel),
            new("ArchiveImage", "Архив", archiveViewModel),
            new("InventoryImage", "Инвентарь", inventoryViewModel),
            new("ProfileImage", "Профиль", profileViewModel)
        ];

        _settingsViewModel = settingsViewModel;

        _selectedNavigationModel = _navigationOptions.First();
        _currentViewModel = _navigationOptions.First().Page;
        _isSettingsChecked = false;

        _userName = USERNAME;
        _steamId = STEAM_ID;
        _isUserLogin = false;

        _logInCommand = new(DoLogInCommand);
        _logOutCommand = new(DoLogOutCommand);

        _userModel.PropertyChanged += UserChangedHandler;
    }

    #endregion Constructor

    #region Methods

    private void DoLogInCommand()
    {
        _apiClient.LogIn();
    }

    private void DoLogOutCommand()
    {
        _apiClient.LogOut();
    }

    private void UserChangedHandler(object? sender, PropertyChangedEventArgs e)
    {
        IsUserLogin = _userModel.User is not null;
        UserName = _userModel.User?.Nickname ?? USERNAME;
        SteamId = _userModel.User is null ? STEAM_ID : $"{STEAM_ID}: {_userModel.User.SteamId}";
        ImageUrl = _userModel.User?.ImageUrlFull;
    }

    #endregion Methods
}