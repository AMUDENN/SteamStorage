namespace SteamStorageAPI.Utilities;

public class TokenHandler : DelegatingHandler
{
    #region Fields

    private readonly ApiClient _apiClient;

    #endregion Fields

    #region Constructor

    public TokenHandler(
        ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    #endregion Constructor

    #region Methods

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(_apiClient.Token))
            request.Headers.Authorization = new("Bearer", _apiClient.Token);

        return await base.SendAsync(request, cancellationToken);
    }

    #endregion Methods
}