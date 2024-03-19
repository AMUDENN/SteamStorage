﻿using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
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

    public RelayCommand<ArchiveGroupModel> DeleteArchiveGroupCommand { get; }

    #endregion Commands

    #region Constructor

    public ArchiveGroupsModel(
        ApiClient apiClient,
        UserModel userModel)
    {
        _apiClient = apiClient;

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

    private void DoDeleteArchiveGroupCommand(ArchiveGroupModel? group)
    {

    }

    public void UpdateGroups()
    {
        GetGroups();
    }

    private async void GetGroups()
    {
        ArchiveGroups.ArchiveGroupsResponse? groupsResponses =
            await _apiClient.GetAsync<ArchiveGroups.ArchiveGroupsResponse, ArchiveGroups.GetArchiveGroupsRequest>(
                ApiConstants.ApiControllers.ArchiveGroups,
                ApiConstants.ApiMethods.GetArchiveGroups,
                new(null, null));
        if (groupsResponses is null) return;
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