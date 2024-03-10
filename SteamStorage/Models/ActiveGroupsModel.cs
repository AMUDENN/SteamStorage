using System.Collections.Generic;
using System.Linq;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
using SteamStorageAPI;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Utilities;

namespace SteamStorage.Models;

public class ActiveGroupsModel : ModelBase
{
    #region Fields

    private readonly ApiClient _apiClient;

    private List<BaseGroupModel> _activeGroupModels;

    #endregion Fields

    public List<BaseGroupModel> ActiveGroupModels
    {
        get => _activeGroupModels;
        private set => SetProperty(ref _activeGroupModels, value);
    }

    #region Constructor

    public ActiveGroupsModel(
        ApiClient apiClient,
        UserModel userModel)
    {
        _apiClient = apiClient;

        _activeGroupModels = [];

        userModel.UserChanged += UserChangedHandler;
    }

    #endregion Constructor

    #region Methods

    private void UserChangedHandler(object? sender)
    {
        GetGames();
    }

    private async void GetGames()
    {
        ActiveGroups.ActiveGroupsResponse? groupsResponses =
            await _apiClient.GetAsync<ActiveGroups.ActiveGroupsResponse, ActiveGroups.GetActiveGroupsRequest>(
                ApiConstants.ApiControllers.Games,
                "GetGames",
                new(null, null));
        if (groupsResponses is null) return;
        ActiveGroupModels = groupsResponses.ActiveGroups.Select(x => new BaseGroupModel(x.Id, x.Title)).ToList();
    }

    #endregion Methods
}
