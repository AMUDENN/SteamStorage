using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
using SteamStorageAPI;

namespace SteamStorage.Models;

public class ListItemsModel : ModelBase
{
    #region Fields

    private readonly ApiClient _apiClient;

    private GameModel? _selectedGameModel;
    private bool _isAllGamesChecked;

    #endregion Fields

    public GameModel? SelectedGameModel
    {
        get => _selectedGameModel;
        set
        {
            SetProperty(ref _selectedGameModel, value);
            if (value is null) return;
            IsAllGamesChecked = false;
        }
    }

    public bool IsAllGamesChecked
    {
        get => _isAllGamesChecked;
        set
        {
            SetProperty(ref _isAllGamesChecked, value);
            if (!value) return;
            SelectedGameModel = null;
        }
    }

    #region Constructor

    public ListItemsModel(ApiClient apiClient)
    {
        _apiClient = apiClient;

        IsAllGamesChecked = true;
    }

    #endregion Constructor
}
