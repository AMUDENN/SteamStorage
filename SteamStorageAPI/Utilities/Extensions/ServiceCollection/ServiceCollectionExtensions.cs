using Microsoft.Extensions.DependencyInjection;
using SteamStorageAPI.Services.AuthorizationService;
using SteamStorageAPI.Services.Logger.LoggerService;
using SteamStorageAPI.Services.Ping.PingService;
using SteamStorageAPI.Utilities.DelegatingHandlers;
using SteamStorageAPI.Utilities.Extensions.ServiceCollection.Options;

namespace SteamStorageAPI.Utilities.Extensions.ServiceCollection;

public static class ServiceCollectionExtensions
{
    #region Methods

    public static IServiceCollection AddSteamStorageApi(
        this IServiceCollection services,
        Action<SteamStorageApiOptions>? configureOptions)
    {
        SteamStorageApiOptions options = new();
        configureOptions?.Invoke(options);

        //Main HttpClient
        services.AddHttpClient(ApiConstants.CLIENT_NAME,
                client =>
                {
                    client.Timeout = TimeSpan.FromSeconds(options.ClientTimeout);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                })
            .AddHttpMessageHandler<TokenHandler>()
            .AddHttpMessageHandler<UnauthorizedHandler>();
        //TokenHandler
        services.AddScoped<TokenHandler>();
        //UnauthorizedHandler
        services.AddScoped<UnauthorizedHandler>();
        //ApiClient
        services.AddSingleton<ApiClient>();

        return services;
    }

    public static IServiceCollection AddSteamStorageLoggerService(
        this IServiceCollection services,
        Action<SteamStorageLoggerOptions>? configureOptions)
    {
        SteamStorageLoggerOptions options = new();
        configureOptions?.Invoke(options);

        //LoggerService
        services.AddSingleton<ILoggerService, LoggerService>(_ =>
            new(options.ProgramName,
                options.DateFormat,
                options.DateTimeFormat,
                options.LogFilesLifetime));

        return services;
    }

    public static IServiceCollection AddSteamStorageAuthorizationService(
        this IServiceCollection services)
    {
        //AuthorizationService
        services.AddScoped<IAuthorizationService, AuthorizationService>();

        return services;
    }

    public static IServiceCollection AddSteamStoragePingService(
        this IServiceCollection services)
    {
        //PingService
        services.AddScoped<IPingService, PingService>(_ =>
            new(ApiConstants.HOST_NAME));

        return services;
    }

    #endregion Methods
}
