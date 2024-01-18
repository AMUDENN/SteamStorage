using System.Net.Http.Json;
using System.Text;

namespace SteamStorageAPI;

public static class ApiClient
{
    #region Fields

    private const string SERVER_ADRESS = "http://localhost:5275/";
    private static readonly HttpClient _client;

    private static string _token;

    #endregion Fields

    #region Constructor

    static ApiClient()
    {
        _client = new();
        _token = string.Empty;

        _client.Timeout = TimeSpan.FromSeconds(value: 2);
        _client.DefaultRequestHeaders.Clear();
        _client.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    #endregion Constructor

    #region Properties

    public static string Token
    {
        get => _token;
        set
        {
            _token = value;
            _client.DefaultRequestHeaders.Authorization = new("Bearer", value);
        }
    }

    #endregion Properties

    #region GET

    private static async Task<TOut?> GetAsync<TOut>(Uri uri, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response = await _client.GetAsync(uri, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TOut>(cancellationToken);
    }

    public static async Task<TOut?> GetAsync<TOut>(string apiMethod, Dictionary<string, string>? args,
        CancellationToken cancellationToken = default)
    {
        return await GetAsync<TOut>(CreateUri(apiMethod, args), cancellationToken);
    }

    #endregion GET

    #region Methods

    private static Uri CreateUri(string apiMethod, Dictionary<string, string>? args = null)
    {
        StringBuilder uri = new(SERVER_ADRESS + apiMethod);
        if (args is not null)
        {
            uri.Append('?');
            foreach (KeyValuePair<string, string> arg in args)
                uri.Append($"{arg.Key}={arg.Value}&");
            uri.Remove(uri.Length - 1, 1);
        }

        return new(uri.ToString());
    }

    #endregion
}