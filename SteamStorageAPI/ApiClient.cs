using System.Diagnostics;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using SteamStorageAPI.ApiEntities.Tools;
using SteamStorageAPI.Services.Logger.LoggerService;
using SteamStorageAPI.Utilities;
using SteamStorageAPI.Utilities.Events;

namespace SteamStorageAPI;

public class ApiClient
{
    #region Events

    public delegate void TokenChangedEventHandler(object? sender, TokenChangedEventArgs args);

    public event TokenChangedEventHandler? TokenChanged;

    #endregion Events

    #region Fields

    private readonly ILoggerService? _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    private string _token;

    #endregion Fields

    #region Constructor

    public ApiClient(
        ILoggerService? logger,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _token = string.Empty;
    }

    #endregion Constructor

    #region Properties

    public string Token
    {
        get => _token;
        set
        {
            _token = value;
            OnTokenChanged(value);
        }
    }

    #endregion Properties

    #region GET

    private async Task<TOut?> GetAsync<TOut>(
        Uri uri,
        CancellationToken cancellationToken = default)
        where TOut : Response
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient(ApiConstants.CLIENT_NAME);
            HttpResponseMessage response = await client.GetAsync(uri, cancellationToken);
            return await response.Content.ReadFromJsonAsync<TOut>(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            Debug.WriteLine($"Task {uri.ToString()} was cancelled");
            return default;
        }
        catch (Exception ex)
        {
            if (_logger is not null)
                await _logger.LogAsync($"ApiException GET \n{uri.ToString()}", ex);
            return default;
        }
    }

    public async Task<TOut?> GetAsync<TOut>(
        ApiConstants.ApiMethods apiMethod,
        CancellationToken cancellationToken = default)
        where TOut : Response
    {
        return await GetAsync<TOut>(
            CreateUri((ApiConstants.ApiControllers)((int)apiMethod / 100), apiMethod),
            cancellationToken);
    }

    public async Task<TOut?> GetAsync<TOut, TIn>(
        ApiConstants.ApiMethods apiMethod,
        TIn? args = null,
        CancellationToken cancellationToken = default)
        where TOut : Response
        where TIn : Request
    {
        return await GetAsync<TOut>(
            CreateUri((ApiConstants.ApiControllers)((int)apiMethod / 100), apiMethod, args),
            cancellationToken);
    }

    #endregion GET

    #region POST

    private async Task PostAsync<TIn>(
        Uri uri,
        TIn? args = null,
        CancellationToken cancellationToken = default)
        where TIn : Request
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient(ApiConstants.CLIENT_NAME);
            await client.PostAsJsonAsync(uri, args, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            Debug.WriteLine($"Task {uri.ToString()} was cancelled");
        }
        catch (Exception ex)
        {
            if (_logger is not null)
                await _logger.LogAsync($"ApiException POST \n{uri.ToString()}", ex);
        }
    }

    public async Task PostAsync<TIn>(
        ApiConstants.ApiMethods apiMethod,
        TIn? args = null,
        CancellationToken cancellationToken = default)
        where TIn : Request
    {
        await PostAsync(
            CreateUri((ApiConstants.ApiControllers)((int)apiMethod / 100), apiMethod),
            args,
            cancellationToken);
    }

    public async Task PostAsync(
        ApiConstants.ApiMethods apiMethod,
        CancellationToken cancellationToken = default)
    {
        await PostAsync<Request>(
            CreateUri((ApiConstants.ApiControllers)((int)apiMethod / 100), apiMethod),
            null,
            cancellationToken);
    }

    #endregion POST
    
    #region PUT
    
    private async Task PutAsync<TIn>(
        Uri uri,
        TIn? args = null,
        CancellationToken cancellationToken = default)
        where TIn : Request
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient(ApiConstants.CLIENT_NAME);
            await client.PutAsJsonAsync(uri, args, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            Debug.WriteLine($"Task {uri.ToString()} was cancelled");
        }
        catch (Exception ex)
        {
            if (_logger is not null)
                await _logger.LogAsync($"ApiException PUT \n{uri.ToString()}", ex);
        }
    }
    
    public async Task PutAsync<TIn>(
        ApiConstants.ApiMethods apiMethod,
        TIn? args = null,
        CancellationToken cancellationToken = default)
        where TIn : Request
    {
        await PutAsync(
            CreateUri((ApiConstants.ApiControllers)((int)apiMethod / 100), apiMethod),
            args,
            cancellationToken);
    }

    public async Task PutAsync(
        ApiConstants.ApiMethods apiMethod,
        CancellationToken cancellationToken = default)
    {
        await PutAsync<Request>(
            CreateUri((ApiConstants.ApiControllers)((int)apiMethod / 100), apiMethod),
            null,
            cancellationToken);
    }
    
    #endregion PUT

    #region DELETE

    private async Task DeleteAsync<TIn>(
        Uri uri,
        TIn? args = null,
        CancellationToken cancellationToken = default)
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
        catch (OperationCanceledException)
        {
            Debug.WriteLine($"Task {uri.ToString()} was cancelled");
        }
        catch (Exception ex)
        {
            if (_logger is not null)
                await _logger.LogAsync($"ApiException DELETE \n{uri.ToString()}", ex);
        }
    }

    public async Task DeleteAsync<TIn>(
        ApiConstants.ApiMethods apiMethod,
        TIn? args = null,
        CancellationToken cancellationToken = default)
        where TIn : Request
    {
        await DeleteAsync(
            CreateUri((ApiConstants.ApiControllers)((int)apiMethod / 100), apiMethod),
            args,
            cancellationToken);
    }

    public async Task DeleteAsync(
        ApiConstants.ApiMethods apiMethod,
        CancellationToken cancellationToken = default)
    {
        await DeleteAsync<Request>(
            CreateUri((ApiConstants.ApiControllers)((int)apiMethod / 100), apiMethod),
            null,
            cancellationToken);
    }

    #endregion DELETE

    #region Methods

    private static string CreateStringUri(
        ApiConstants.ApiControllers apiController,
        ApiConstants.ApiMethods apiMethod)
    {
        return $"{ApiConstants.SERVER_ADDRESS}api/{apiController}/{apiMethod}";
    }

    private static Uri CreateUri(
        ApiConstants.ApiControllers apiController,
        ApiConstants.ApiMethods apiMethod)
    {
        return new(CreateStringUri(apiController, apiMethod));
    }

    private static Uri CreateUri<TIn>(
        ApiConstants.ApiControllers apiController,
        ApiConstants.ApiMethods apiMethod,
        TIn? args = null)
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

    private void OnTokenChanged(
        string token)
    {
        TokenChanged?.Invoke(this, new(token));
    }

    #endregion Methods
}
