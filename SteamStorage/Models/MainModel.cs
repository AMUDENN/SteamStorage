﻿using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Actives;
using SteamStorage.Models.Archives;
using SteamStorage.Models.Home;
using SteamStorage.Models.Tools;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.Services.NotificationService;
using SteamStorage.Utilities.Events.ListItems;
using SteamStorage.ViewModels;
using SteamStorage.ViewModels.Actives;
using SteamStorage.ViewModels.Archives;
using SteamStorage.ViewModels.Home;
using SteamStorage.ViewModels.Inventory;
using SteamStorage.ViewModels.Profile;
using SteamStorage.ViewModels.Settings;
using SteamStorage.ViewModels.Tools;
using SteamStorageAPI.SDK.Services.AuthorizationService;

namespace SteamStorage.Models;

public class MainModel : ModelBase
{
    #region Constants

    private const string USERNAME = "Username";
    private const string STEAM_ID = "SteamID";

    #endregion Constants

    #region Fields

    private readonly UserModel _userModel;
    private readonly IAuthorizationService _authorizationService;
    private readonly INotificationService _notificationService;

    private string? _imageUrl;
    private string _userName;
    private string _steamId;
    private bool _isUserLogin;

    private ViewModelBase _currentViewModel;
    private readonly ViewModelBase _settingsViewModel;
    private readonly ViewModelBase _defaultViewModel;

    private NavigationModel? _selectedNavigationModel;
    private bool _isSettingsChecked;

    private NavigationModel? _lastNavigationModel;

    #endregion Fields

    #region Properties

    public string? ImageUrl
    {
        get => _imageUrl;
        private set => SetProperty(ref _imageUrl, value);
    }

    public string UserName
    {
        get => _userName;
        private set => SetProperty(ref _userName, value);
    }

    public string SteamId
    {
        get => _steamId;
        private set => SetProperty(ref _steamId, value);
    }

    public bool IsUserLogin
    {
        get => _isUserLogin;
        private set => SetProperty(ref _isUserLogin, value);
    }

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        private set => SetProperty(ref _currentViewModel, value);
    }

    public IEnumerable<NavigationModel> NavigationOptions { get; }

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

    public RelayCommand LogInCommand { get; }

    public RelayCommand LogOutCommand { get; }

    #endregion Commands

    #region Constructor

    public MainModel(
        UserModel userModel,
        IAuthorizationService authorizationService,
        INotificationService notificationService,
        HomeViewModel homeViewModel,
        ActivesViewModel activesViewModel,
        ArchivesViewModel archivesViewModel,
        InventoryViewModel inventoryViewModel,
        ProfileViewModel profileViewModel,
        SettingsViewModel settingsViewModel,
        DefaultViewModel defaultViewModel,
        ListItemsModel listItemsModel,
        ActiveEditModel activeEditModel,
        ArchiveEditModel archiveEditModel)
    {
        _userModel = userModel;
        _authorizationService = authorizationService;
        _notificationService = notificationService;

        NavigationOptions =
        [
            new("HomeVectorImage", "Главная", homeViewModel),
            new("ActivesVectorImage", "Активы", activesViewModel),
            new("ArchiveVectorImage", "Архив", archivesViewModel),
            new("InventoryVectorImage", "Инвентарь", inventoryViewModel),
            new("ProfileVectorImage", "Профиль", profileViewModel)
        ];

        _settingsViewModel = settingsViewModel;
        _defaultViewModel = defaultViewModel;
        
        _currentViewModel = defaultViewModel;
        _isSettingsChecked = false;

        _userName = USERNAME;
        _steamId = STEAM_ID;
        _isUserLogin = false;

        LogInCommand = new(DoLogInCommand);
        LogOutCommand = new(DoLogOutCommand);

        userModel.UserChanged += UserChangedHandler;

        authorizationService.AuthorizationCompleted += AuthorizationCompletedHandler;
        authorizationService.LogOutCompleted += LogOutCompletedHandler;

        listItemsModel.AddToActives += AddToActivesHandler;
        listItemsModel.AddToArchive += AddToArchivesHandler;

        activeEditModel.GoingBack += GoingBackHandler;
        archiveEditModel.GoingBack += GoingBackHandler;
    }

    #endregion Constructor

    #region Methods

    private void UserChangedHandler(object? sender)
    {
        IsUserLogin = _userModel.User is not null;
        UserName = _userModel.User?.Nickname ?? USERNAME;
        SteamId = _userModel.User is null ? STEAM_ID : $"{STEAM_ID}: {_userModel.User.SteamId}";
        ImageUrl = _userModel.User?.ImageUrlFull;

        SelectedNavigationModel = null;
        IsSettingsChecked = false;

        if (_userModel.User is not null)
        {
            SelectedNavigationModel = NavigationOptions.FirstOrDefault(x => x.Title == _userModel.User.StartPage);
        }
        else
        {
            SelectedNavigationModel = null;
            IsSettingsChecked = false;
            CurrentViewModel = _defaultViewModel;
        }
    }

    private async void AuthorizationCompletedHandler(object? sender)
    {
        await _notificationService.ShowAsync("Авторизация", "Авторизация прошла успешно");
    }
    
    private async void LogOutCompletedHandler(object? sender)
    {
        await _notificationService.ShowAsync("Выход", "Вы вышли из аккаунта");
    }

    private void AddToActivesHandler(object? sender, AddToActivesEventArgs args)
    {
        NavigationModel? navigationModel = FindViewModel(typeof(ActivesViewModel));

        _lastNavigationModel = SelectedNavigationModel;

        SelectedNavigationModel = navigationModel;
    }

    private void AddToArchivesHandler(object? sender, AddToArchiveEventArgs args)
    {
        NavigationModel? navigationModel = FindViewModel(typeof(ArchivesViewModel));

        _lastNavigationModel = SelectedNavigationModel;

        SelectedNavigationModel = navigationModel;
    }

    private void GoingBackHandler(object? sender)
    {
        if (_lastNavigationModel is not null 
            && SelectedNavigationModel != _lastNavigationModel 
            && _lastNavigationModel?.Page.GetType() == typeof(HomeViewModel))
            SelectedNavigationModel = _lastNavigationModel;
        _lastNavigationModel = null;
    }

    private void DoLogInCommand()
    {
        _authorizationService.LogIn();
    }

    private void DoLogOutCommand()
    {
        _authorizationService.LogOut();
    }

    private NavigationModel? FindViewModel(Type type)
    {
        return NavigationOptions.FirstOrDefault(x => x.Page.GetType() == type);
    }

    #endregion Methods
}
