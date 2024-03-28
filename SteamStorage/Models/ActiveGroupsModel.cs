using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.BaseModels;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Models.UtilityModels.BaseModels;
using SteamStorage.Services.DialogService;
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
    private readonly IDialogService _dialogService;

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
        IDialogService dialogService)
    {
        _apiClient = apiClient;
        _dialogService = dialogService;

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

    private async Task DoDeleteActiveGroupCommand(ActiveGroupModel? group)
    {
        if (group is null) return;
        
        bool result = await _dialogService.ShowDialog(
            $"Вы уверены, что хотите удалить группу: «{group.Title}»?",
            BaseDialogModel.MessageType.Question,
            BaseDialogModel.MessageButtons.OkCancel);
        
        if (!result) return;

        await _apiClient.DeleteAsync(
            ApiConstants.ApiMethods.DeleteActiveGroup,
            new ActiveGroups.DeleteActiveGroupRequest(group.GroupId));

        GetGroups();
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
        if (groupsResponses?.ActiveGroups is null) return;
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
