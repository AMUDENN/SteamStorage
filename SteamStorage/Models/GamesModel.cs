using System.Collections.Generic;
using System.Linq;
using SteamStorage.Models.Tools;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorageAPI.SDK.ApiClient;
using SteamStorageAPI.SDK.ApiEntities;
using SteamStorageAPI.SDK.Utilities.ApiControllers;

namespace SteamStorage.Models;

public class GamesModel : ModelBase
{
    #region Fields

    private readonly IApiClient _apiClient;

    private List<GameModel> _gameModels;

    #endregion Fields

    public List<GameModel> GameModels
    {
        get => _gameModels;
        private set => SetProperty(ref _gameModels, value);
    }

    #region Constructor

    public GamesModel(
        IApiClient apiClient,
        UserModel userModel)
    {
        _apiClient = apiClient;

        _gameModels = [];

        userModel.UserChanged += UserChangedHandler;
    }

    #endregion Constructor

    #region Methods

    private void UserChangedHandler(object? sender)
    {
        GetGamesAsync();
    }

    private async void GetGamesAsync()
    {
        Games.GamesResponse? gamesResponse =
            await _apiClient.GetAsync<Games.GamesResponse>(
                ApiConstants.ApiMethods.GetGames);
        if (gamesResponse?.Games is null) return;
        GameModels = gamesResponse.Games.Select(x => new GameModel(x.Id, x.GameIconUrl, x.Title)).ToList();
    }

    #endregion Methods
}