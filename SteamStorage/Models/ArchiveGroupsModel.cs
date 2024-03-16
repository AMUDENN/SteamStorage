﻿using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
using SteamStorageAPI;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Utilities;

namespace SteamStorage.Models;

public class ArchiveGroupsModel : ModelBase
{
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

    }

    private void DoOpenArchivesCommand(ArchiveGroupModel? group)
    {

    }

    private void DoAddArchiveGroupCommand()
    {

    }

    private void DoEditArchiveGroupCommand(ArchiveGroupModel? group)
    {

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

    #endregion Methods
}
