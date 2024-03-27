using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Models.UtilityModels.BaseModels;
using SteamStorage.Utilities.Events.Actives;
using SteamStorageAPI;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Utilities;

namespace SteamStorage.Models;

public class ActiveGroupsModel : ModelBase
{
    #region Events

    public delegate void AddActiveEventHandler(object? sender, AddActiveEventArgs args);

    public event AddActiveEventHandler? AddActive;

    public delegate void OpenActivesEventHandler(object? sender, OpenActivesEventArgs args);

    public event OpenActivesEventHandler? OpenActives;

    public delegate void EditActiveGroupEventHandler(object? sender, EditActiveGroupEventArgs args);

    public event EditActiveGroupEventHandler? EditActiveGroup;

    #endregion Events

    #region Fields

    private readonly ApiClient _apiClient;

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

    public RelayCommand<ActiveGroupModel> DeleteActiveGroupCommand { get; }

    #endregion Commands

    #region Constructor

    public ActiveGroupsModel(
        ApiClient apiClient,
        UserModel userModel)
    {
        _apiClient = apiClient;

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
        GetGroups();
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

    private void DoDeleteActiveGroupCommand(ActiveGroupModel? group)
    {
        
    }

    public void UpdateGroups()
    {
        GetGroups();
    }

    private async void GetGroups()
    {
        ActiveGroups.ActiveGroupsResponse? groupsResponses =
            await _apiClient.GetAsync<ActiveGroups.ActiveGroupsResponse, ActiveGroups.GetActiveGroupsRequest>(
                ApiConstants.ApiMethods.GetActiveGroups,
                new(null, null));
        if (groupsResponses is null) return;
        ActiveGroupModels = groupsResponses.ActiveGroups.Select(x => new BaseGroupModel(x.Id, x.Title)).ToList();
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

    #endregion Methods
}
