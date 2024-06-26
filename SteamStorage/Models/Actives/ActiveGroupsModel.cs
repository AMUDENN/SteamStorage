﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.Models.Tools.UtilityModels.BaseModels;
using SteamStorage.Services.DialogService;
using SteamStorage.Services.NotificationService;
using SteamStorage.Utilities.Dialog;
using SteamStorage.Utilities.Events.Actives;
using SteamStorageAPI.SDK;
using SteamStorageAPI.SDK.ApiEntities;
using SteamStorageAPI.SDK.Utilities;

namespace SteamStorage.Models.Actives;

public class ActiveGroupsModel : ModelBase
{
    #region Events

    public delegate void AddActiveEventHandler(object? sender, AddActiveEventArgs args);

    public event AddActiveEventHandler? AddActive;

    public delegate void OpenActivesEventHandler(object? sender, OpenActivesEventArgs args);

    public event OpenActivesEventHandler? OpenActives;

    public delegate void EditActiveGroupEventHandler(object? sender, EditActiveGroupEventArgs args);

    public event EditActiveGroupEventHandler? EditActiveGroup;
    
    public delegate void DeleteActiveGroupEventHandler(object? sender);

    public event DeleteActiveGroupEventHandler? DeleteActiveGroup;

    #endregion Events

    #region Fields

    private readonly ApiClient _apiClient;
    private readonly IDialogService _dialogService;
    private readonly INotificationService _notificationService;

    private List<BaseGroupModel> _activeGroupModels;

    #endregion Fields

    #region Properties

    public List<BaseGroupModel> ActiveGroupModels
    {
        get => _activeGroupModels;
        private set => SetProperty(ref _activeGroupModels, value);
    }

    #endregion Properties

    #region Commands

    public RelayCommand<ActiveGroupModel> AddActiveCommand { get; }

    public RelayCommand<ActiveGroupModel> OpenActivesCommand { get; }

    public RelayCommand AddActiveGroupCommand { get; }

    public RelayCommand<ActiveGroupModel> EditActiveGroupCommand { get; }

    public AsyncRelayCommand<ActiveGroupModel> DeleteActiveGroupCommand { get; }

    #endregion Commands

    #region Constructor

    public ActiveGroupsModel(
        ApiClient apiClient,
        UserModel userModel,
        IDialogService dialogService,
        INotificationService notificationService)
    {
        _apiClient = apiClient;
        _dialogService = dialogService;
        _notificationService = notificationService;

        _activeGroupModels = [];

        userModel.UserChanged += UserChangedHandler;

        AddActiveCommand = new(DoAddActiveCommand);
        OpenActivesCommand = new(DoOpenActivesCommand);
        AddActiveGroupCommand = new(DoAddActiveGroupCommand);
        EditActiveGroupCommand = new(DoEditActiveGroupCommand);
        DeleteActiveGroupCommand = new(DoDeleteActiveGroupCommand);
    }

    #endregion Constructor

    #region Methods

    private void UserChangedHandler(object? sender)
    {
        GetGroupsAsync();
    }

    private void DoAddActiveCommand(ActiveGroupModel? group)
    {
        OnAddActive(group);
    }

    private void DoOpenActivesCommand(ActiveGroupModel? group)
    {
        OnOpenActives(group);
    }

    private void DoAddActiveGroupCommand()
    {
        OnEditActiveGroup(null);
    }

    private void DoEditActiveGroupCommand(ActiveGroupModel? group)
    {
        OnEditActiveGroup(group);
    }

    private async Task DoDeleteActiveGroupCommand(ActiveGroupModel? group, CancellationToken cancellationToken)
    {
        if (group is null) return;
        
        bool result = await _dialogService.ShowDialogAsync(
            $"Вы уверены, что хотите удалить группу: «{group.Title}»?",
            DialogUtility.MessageType.Question,
            DialogUtility.MessageButtons.OkCancel);
        
        if (!result) return;

        await _apiClient.DeleteAsync(
            ApiConstants.ApiMethods.DeleteActiveGroup,
            new ActiveGroups.DeleteActiveGroupRequest(group.GroupId),
            cancellationToken);
        
        await _notificationService.ShowAsync("Удаление группы",
            $"Вы отправили запрос на удаление группы: {group.Title}", 
            cancellationToken: cancellationToken);

        GetGroupsAsync();

        OnDeleteActiveGroup();
    }

    public void UpdateGroups()
    {
        GetGroupsAsync();
    }

    private async void GetGroupsAsync()
    {
        ActiveGroups.ActiveGroupsResponse? groupsResponses =
            await _apiClient.GetAsync<ActiveGroups.ActiveGroupsResponse, ActiveGroups.GetActiveGroupsRequest>(
                ApiConstants.ApiMethods.GetActiveGroups,
                new(null, null));
        if (groupsResponses?.ActiveGroups is null) return;
        ActiveGroupModels = groupsResponses.ActiveGroups
            .Select(x => new BaseGroupModel(x.Id, x.Title, x.Colour))
            .ToList();
    }

    private void OnAddActive(ActiveGroupModel? group)
    {
        AddActive?.Invoke(this, new(group));
    }

    private void OnOpenActives(ActiveGroupModel? group)
    {
        OpenActives?.Invoke(this, new(group));
    }

    private void OnEditActiveGroup(ActiveGroupModel? group)
    {
        EditActiveGroup?.Invoke(this, new(group));
    }
    
    private void OnDeleteActiveGroup()
    {
        DeleteActiveGroup?.Invoke(this);
    }

    #endregion Methods
}
