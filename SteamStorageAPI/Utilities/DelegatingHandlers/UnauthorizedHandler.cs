using System.Net;
using SteamStorageAPI.Services.Logger.LoggerService;

namespace SteamStorageAPI.Utilities.DelegatingHandlers;

public class UnauthorizedHandler : DelegatingHandler
{
    #region Fields

    private readonly ILoggerService _logger;

    private readonly ApiClient _apiClient;

    #endregion Fields

    #region Constructor

    public UnauthorizedHandler(
        ILoggerService logger,
        ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    #endregion Constructor

    #region Methods

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.Unauthorized) return response;

        _apiClient.Token = string.Empty;
        await _logger.LogAsync("Unauthorized request");

        return response;
    }

    #endregion Methods
}
