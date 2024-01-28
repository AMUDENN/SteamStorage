using CommunityToolkit.Mvvm.ComponentModel;
using SteamStorageAPI;

namespace SteamStorage.Models;

public class ListItemsModel : ObservableObject
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