namespace SteamStorageAPI.Services.Logger.LoggerService;

public interface ILoggerService
{
    public Task LogAsync(string message);

    public Task LogAsync(Exception exception);

    public Task LogAsync(string message, Exception exception);
}
