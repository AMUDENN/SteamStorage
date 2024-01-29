using SteamStorage.Models.Tools;
using SteamStorageAPI;

namespace SteamStorage.Models;

public class ListItemsModel : ModelBase
{
    #region Fields

    private readonly ApiClient _apiClient;

    #endregion Fields

    #region Constructor

    public ListItemsModel(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    #endregion
}
