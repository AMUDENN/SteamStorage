using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.BaseModels;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Models.UtilityModels.BaseModels;
using SteamStorage.Services.DialogService;
using SteamStorage.Utilities.Events.Archives;
using SteamStorageAPI;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Utilities;

namespace SteamStorage.Models;

public class ArchiveGroupsModel : ModelBase
{
    #region Events

    public delegate void AddArchiveEventHandler(object? sender, AddArchiveEventArgs args);

    public event AddArchiveEventHandler? AddArchive;

    public delegate void OpenArchivesEventHandler(object? sender, OpenArchivesEventArgs args);

    public event OpenArchivesEventHandler? OpenArchives;

    public delegate void EditArchiveGroupEventHandler(object? sender, EditArchiveGroupEventArgs args);

    public event EditArchiveGroupEventHandler? EditArchiveGroup;

    #endregion Events

    #region Fields

    private readonly ApiClient _apiClient;
    private readonly IDialogService _dialogService;

    private List<BaseGroupModel> _archiveGroupModels;

    #endregion Fields

    #region Properties

    public List<BaseGroupModel> ArchiveGroupModels
    {
        get => _archiveGroupModels;
        private set => SetProperty(ref _archiveGroupModels, value);
    }

    #endregion Properties

    #region Commands

    public RelayCommand<ArchiveGroupModel> OpenArchivesCommand { get; }

    public RelayCommand<ArchiveGroupModel> AddArchiveCommand { get; }

    public RelayCommand AddArchiveGroupCommand { get; }

    public RelayCommand<ArchiveGroupModel> EditArchiveGroupCommand { get; }

    public AsyncRelayCommand<ArchiveGroupModel> DeleteArchiveGroupCommand { get; }

    #endregion Commands

    #region Constructor

    public ArchiveGroupsModel(
        ApiClient apiClient,
        UserModel userModel,
        IDialogService dialogService)
    {
        _apiClient = apiClient;
        _dialogService = dialogService;

        _archiveGroupModels = [];

        userModel.UserChanged += UserChangedHandler;

        AddArchiveCommand = new(DoAddArchiveCommand);
        OpenArchivesCommand = new(DoOpenArchivesCommand);
        AddArchiveGroupCommand = new(DoAddArchiveGroupCommand);
        EditArchiveGroupCommand = new(DoEditArchiveGroupCommand);
        DeleteArchiveGroupCommand = new(DoDeleteArchiveGroupCommand);
    }

    #endregion Constructor

    #region Methods

    private void UserChangedHandler(object? sender)
    {
        GetGroups();
    }

    private void DoAddArchiveCommand(ArchiveGroupModel? group)
    {
        OnAddArchive(group);
    }

    private void DoOpenArchivesCommand(ArchiveGroupModel? group)
    {
        OnOpenArchives(group);
    }

    private void DoAddArchiveGroupCommand()
    {
        OnEditArchiveGroup(null);
    }

    private void DoEditArchiveGroupCommand(ArchiveGroupModel? group)
    {
        OnEditArchiveGroup(group);
    }

    private async Task DoDeleteArchiveGroupCommand(ArchiveGroupModel? group)
    {
        if (group is null) return;
        
        bool result = await _dialogService.ShowDialog(
            $"Вы уверены, что хотите удалить группу: «{group.Title}»?",
            BaseDialogModel.MessageType.Question,
            BaseDialogModel.MessageButtons.OkCancel);
        
        if (!result) return;

        await _apiClient.DeleteAsync(
            ApiConstants.ApiMethods.DeleteArchiveGroup,
            new ArchiveGroups.DeleteArchiveGroupRequest(group.GroupId));

        GetGroups();
    }

    public void UpdateGroups()
    {
        GetGroups();
    }

    private async void GetGroups()
    {
        ArchiveGroups.ArchiveGroupsResponse? groupsResponses =
            await _apiClient.GetAsync<ArchiveGroups.ArchiveGroupsResponse, ArchiveGroups.GetArchiveGroupsRequest>(
                ApiConstants.ApiMethods.GetArchiveGroups,
                new(null, null));
        if (groupsResponses?.ArchiveGroups is null) return;
        ArchiveGroupModels = groupsResponses.ArchiveGroups.Select(x => new BaseGroupModel(x.Id, x.Title)).ToList();
    }

    private void OnAddArchive(ArchiveGroupModel? group)
    {
        AddArchive?.Invoke(this, new(group));
    }

    private void OnOpenArchives(ArchiveGroupModel? group)
    {
        OpenArchives?.Invoke(this, new(group));
    }

    private void OnEditArchiveGroup(ArchiveGroupModel? group)
    {
        EditArchiveGroup?.Invoke(this, new(group));
    }

    #endregion Methods
}
