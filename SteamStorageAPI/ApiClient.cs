using System.Diagnostics;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.SignalR.Client;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.ApiEntities.Tools;
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
            await _logger.LogAsync($"ApiException GET \n{uri.ToString()}", ex);
            return default;
        }
    }

    public async Task<TOut?> GetAsync<TOut>(ApiConstants.ApiControllers apiController, string apiMethod,
        CancellationToken cancellationToken = default)
    {
        return await GetAsync<TOut>(CreateUri(apiController, apiMethod), cancellationToken);
    }

    public async Task<TOut?> GetAsync<TOut, TIn>(ApiConstants.ApiControllers apiController, string apiMethod,
        TIn? args = null, CancellationToken cancellationToken = default)
        where TIn : Request
    {
        return await GetAsync<TOut>(CreateUri(apiController, apiMethod, args), cancellationToken);
    }

    #endregion GET
    
    #region POST

    private async Task PostAsync<TIn>(Uri uri, TIn? args = null, CancellationToken cancellationToken = default)
        where TIn : Request
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient(ApiConstants.CLIENT_NAME);
            await client.PostAsJsonAsync(uri, args, cancellationToken);
        }
        catch (Exception ex)
        {
            await _logger.LogAsync($"ApiException POST \n{uri.ToString()}", ex);
        }
    }

    public async Task PostAsync<TIn>(ApiConstants.ApiControllers apiController, string apiMethod, TIn? args = null,
        CancellationToken cancellationToken = default) where TIn : Request
    {
        await PostAsync(CreateUri(apiController, apiMethod), args, cancellationToken);
    }

    #endregion POST
    
    #region DELETE
    
    private async Task DeleteAsync<TIn>(Uri uri, TIn? args = null, CancellationToken cancellationToken = default)
        where TIn : Request
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient(ApiConstants.CLIENT_NAME);
            HttpRequestMessage request = new()
            {
                Content = JsonContent.Create(args),
                Method = HttpMethod.Delete,
                RequestUri = uri
            };
            await client.SendAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            await _logger.LogAsync($"ApiException POST \n{uri.ToString()}", ex);
        }
    }
    
    public async Task DeleteAsync<TIn>(ApiConstants.ApiControllers apiController, string apiMethod, TIn? args = null,
        CancellationToken cancellationToken = default) where TIn : Request
    {
        await DeleteAsync(CreateUri(apiController, apiMethod), args, cancellationToken);
    }
    
    #endregion DELETE

    #region Methods

    private static string CreateStringUri(ApiConstants.ApiControllers apiController, string apiMethod)
    {
        return $"{ApiConstants.SERVER_ADRESS}api/{apiController}/{apiMethod}";
    }

    private static Uri CreateUri(ApiConstants.ApiControllers apiController, string apiMethod)
    {
        return new(CreateStringUri(apiController, apiMethod));
    }

    private static Uri CreateUri<TIn>(ApiConstants.ApiControllers apiController, string apiMethod, TIn? args = null)
        where TIn : Request
    {
        StringBuilder uri = new(CreateStringUri(apiController, apiMethod));
        if (args is null) return new(uri.ToString());

        Type type = args.GetType();
        PropertyInfo[] properties = type.GetProperties();

        uri.Append('?');
        foreach (PropertyInfo property in properties)
        {
            object? value = property.GetValue(args);
            if (value is null) continue;
            
            if (property.PropertyType == typeof(DateTime))
                uri.Append($"{property.Name}={Convert.ToDateTime(value).ToString(ApiConstants.API_DATE_FORMAT)}&");
            else
                uri.Append($"{property.Name}={value}&");
        }

        uri.Remove(uri.Length - 1, 1);

        return new(uri.ToString());
    }

    private void OnTokenChanged(string token)
    {
        TokenChanged?.Invoke(this, new(token));
    }

    #endregion Methods
}