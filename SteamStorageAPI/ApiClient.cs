using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.SignalR.Client;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Events;

namespace SteamStorageAPI;

public class ApiClient
{
    #region Constants

    public const string CLIENT_NAME = "MainClient";
    private const string SERVER_ADRESS = "http://localhost:5275/";
    private const string TOKENHUB_ADRESS = "http://localhost:5245/token-hub";

    #endregion Constants

    #region Enums

    public enum ApiControllers
    {
        Authorize,
        Users,
        Games
    }

    #endregion Enums

    #region Events

    public delegate void TokenChangedEventHandler(object sender, TokenChangedEventArgs args);

    public event TokenChangedEventHandler? TokenChanged;

    #endregion Events

    #region Fields

    private const string TOKEN_METHOD_NAME = "Token";
    private const string JOIN_GROUP_METHOD_NAME = "JoinGroup";

    private readonly IHttpClientFactory _httpClientFactory;

    private string _token;

    #endregion Fields

    #region Constructor

    public ApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _token = string.Empty;
    }

    #endregion Constructor

    #region Properties

    public string Token
    {
        get => _token;
        private set
        {
            _token = value;
            OnTokenChanged(value);
        }
    }

    #endregion Properties

    #region Authorization

    public async void LogIn()
    {
        Authorize.AuthUrlResponse? authUrlResponse =
            await GetAsync<Authorize.AuthUrlResponse>(ApiControllers.Authorize, "GetAuthUrl");

        if (authUrlResponse is null) return;

        Process.Start(new ProcessStartInfo(authUrlResponse.Url)
        {
            UseShellExecute = true
        });

        HubConnection hubConnection = new HubConnectionBuilder()
            .WithUrl(TOKENHUB_ADRESS)
            .Build();

        hubConnection.On<string>(TOKEN_METHOD_NAME, async token =>
        {
            Token = token;
            await hubConnection.StopAsync();
        });

        await hubConnection.StartAsync();

        await hubConnection.InvokeAsync(JOIN_GROUP_METHOD_NAME, authUrlResponse.Group);
    }

    public void LogOut()
    {
        Token = string.Empty;
    }

    #endregion Authorization

    #region GET

    private async Task<TOut?> GetAsync<TOut>(Uri uri, CancellationToken cancellationToken = default)
    {
        HttpClient client = _httpClientFactory.CreateClient(CLIENT_NAME);
        HttpResponseMessage response = await client.GetAsync(uri, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TOut>(cancellationToken);
    }

    public async Task<TOut?> GetAsync<TOut>(ApiControllers apiController, string apiMethod,
        Dictionary<string, string>? args = null,
        CancellationToken cancellationToken = default)
    {
        return await GetAsync<TOut>(CreateUri(apiController, apiMethod, args), cancellationToken);
    }

    public async Task<string> GetStringAsync(ApiControllers apiController, string apiMethod,
        Dictionary<string, string>? args = null,
        CancellationToken cancellationToken = default)
    {
        HttpClient client = _httpClientFactory.CreateClient(CLIENT_NAME);
        return await client.GetStringAsync(CreateUri(apiController, apiMethod, args), cancellationToken);
    }

    #endregion GET

    #region Methods

    private static Uri CreateUri(ApiControllers apiController, string apiMethod,
        Dictionary<string, string>? args = null)
    {
        StringBuilder uri = new($"{SERVER_ADRESS}api/{apiController}/{apiMethod}");
        if (args is not null)
        {
            uri.Append('?');
            foreach (KeyValuePair<string, string> arg in args)
                uri.Append($"{arg.Key}={arg.Value}&");
            uri.Remove(uri.Length - 1, 1);
        }

        Debug.WriteLine(uri.ToString());
        return new(uri.ToString());
    }

    private void OnTokenChanged(string token)
    {
        TokenChanged?.Invoke(this, new(token));
    }

    #endregion
}