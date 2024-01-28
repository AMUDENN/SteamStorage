using CommunityToolkit.Mvvm.ComponentModel;
using SteamStorageAPI;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Utilities;

namespace SteamStorage.Models;

public class UserModel : ObservableObject
{
    #region Fields

    private readonly ApiClient _apiClient;

    private Users.UserResponse? _user;

    #endregion Fields

    #region Properties

    public Users.UserResponse? User
    {
        get => _user;
        private set => SetProperty(ref _user, value);
    }

    #endregion Properties

    #region Constructor

    public UserModel(ApiClient apiClient)
    {
        _apiClient = apiClient;

        _apiClient.TokenChanged += async (s, e) =>
        {
            User = e.IsTokenEmpty
                ? null
                : await _apiClient.GetAsync<Users.UserResponse>(ApiConstants.ApiControllers.Users,
                    "GetCurrentUserInfo");
        };
    }

    #endregion Constructor
}