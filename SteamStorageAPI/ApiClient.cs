using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.SignalR.Client;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Events;
using SteamStorageAPI.Services.LoggerService;
using SteamStorageAPI.Services.PingResult;
using SteamStorageAPI.Services.PingService;
using SteamStorageAPI.Utilities;

namespace SteamStorageAPI;

public class ApiClient
{
    #region Events

    public delegate void TokenChangedEventHandler(object sender, TokenChangedEventArgs args);

    public event TokenChangedEventHandler? TokenChanged;

    #endregion Events

    #region Fields

    private readonly ILoggerService _logger;
    private readonly IPingService _ping;
    private readonly IHttpClientFactory _httpClientFactory;

    private string _token;

    #endregion Fields

    #region Constructor

    public ApiClient(ILoggerService logger, IPingService ping, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _ping = ping;
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
            await GetAsync<Authorize.AuthUrlResponse>(ApiConstants.ApiControllers.Authorize, "GetAuthUrl");

        if (authUrlResponse is null) return;

        Process.Start(new ProcessStartInfo(authUrlResponse.Url)
        {
            UseShellExecute = true
        });

        HubConnection hubConnection = new HubConnectionBuilder()
            .WithUrl(ApiConstants.TOKENHUB_ADRESS)
            .Build();

        hubConnection.On<string>(ApiConstants.TOKEN_METHOD_NAME, async token =>
        {
            Token = token;
            await hubConnection.StopAsync();
        });

        await hubConnection.StartAsync();

        await hubConnection.InvokeAsync(ApiConstants.JOIN_GROUP_METHOD_NAME, authUrlResponse.Group);
    }

    public void LogOut()
    {
        Token = string.Empty;
    }

    #endregion Authorization

    #region PING

    public async Task<PingResult> GetApiPing()
    {
        return await _ping.GetPing(ApiConstants.HOST_NAME);
    }

    #endregion PING

    #region GET

    private async Task<TOut?> GetAsync<TOut>(Uri uri, CancellationToken cancellationToken = default)
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient(ApiConstants.CLIENT_NAME);
            HttpResponseMessage response = await client.GetAsync(uri, cancellationToken);
            return await response.Content.ReadFromJsonAsync<TOut>(cancellationToken);
        }
        catch (Exception ex)
        {
            await _logger.LogAsync($"ApiException \n{uri.ToString()}", ex);
            return default;
        }
    }

    public async Task<TOut?> GetAsync<TOut>(ApiConstants.ApiControllers apiController, string apiMethod,
        Dictionary<string, string>? args = null,
        CancellationToken cancellationToken = default)
    {
        return await GetAsync<TOut>(CreateUri(apiController, apiMethod, args), cancellationToken);
    }

    #endregion GET

    #region Methods

    private static Uri CreateUri(ApiConstants.ApiControllers apiController, string apiMethod,
        Dictionary<string, string>? args = null)
    {
        StringBuilder uri = new($"{ApiConstants.SERVER_ADRESS}api/{apiController}/{apiMethod}");
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