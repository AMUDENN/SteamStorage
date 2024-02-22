using Microsoft.AspNetCore.SignalR.Client;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Utilities;

namespace SteamStorageAPI.Services.AuthorizationService;

public class AuthorizationService : IAuthorizationService
{
    #region Fields

    private readonly ApiClient _apiClient;

    #endregion Fields

    #region Constructor

    public AuthorizationService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    #endregion Constructor

    #region Methods

    public async void LogIn()
    {
        Authorize.AuthUrlResponse? authUrlResponse =
            await _apiClient.GetAsync<Authorize.AuthUrlResponse>(ApiConstants.ApiControllers.Authorize, "GetAuthUrl");

        if (authUrlResponse is null) return;

        UrlUtility.OpenUrl(authUrlResponse.Url);

        HubConnection hubConnection = new HubConnectionBuilder()
            .WithUrl(ApiConstants.TOKENHUB_ADRESS)
            .Build();

        hubConnection.On<string>(ApiConstants.TOKEN_METHOD_NAME, async token =>
        {
            _apiClient.Token = token;
            await hubConnection.StopAsync();
        });

        await hubConnection.StartAsync();

        await hubConnection.InvokeAsync(ApiConstants.JOIN_GROUP_METHOD_NAME, authUrlResponse.Group);
    }

    public void LogOut()
    {
        _apiClient.Token = string.Empty;
    }

    #endregion Methods
}
