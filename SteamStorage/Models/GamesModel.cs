using System.Collections.Generic;
using System.Linq;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
using SteamStorageAPI;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Utilities;

namespace SteamStorage.Models;

public class GamesModel : ModelBase
{
    #region Fields

    private readonly ApiClient _apiClient;
    private readonly UserModel _userModel;

    private List<GameModel> _gameModels;

    #endregion Fields

    public List<GameModel> GameModels
    {
        get => _gameModels;
        private set => SetProperty(ref _gameModels, value);
    }

    #region Constructor

    public GamesModel(ApiClient apiClient, UserModel userModel)
    {
        _apiClient = apiClient;
        _userModel = userModel;

        userModel.UserChanged += (s) => GetGames();
    }

    #endregion Constructor

    #region Methods

    private async void GetGames()
    {
        List<Games.GameResponse>? gamesResponse =
            await _apiClient.GetAsync<List<Games.GameResponse>>(ApiConstants.ApiControllers.Games,
                "GetGames");
        if (gamesResponse is null) return;
        GameModels = gamesResponse.Select(x => new GameModel(x.GameIconUrl, x.Title)).ToList();
    }

    #endregion Methods
}