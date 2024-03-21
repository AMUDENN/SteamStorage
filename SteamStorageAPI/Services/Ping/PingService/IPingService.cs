namespace SteamStorageAPI.Services.Ping.PingService;

public interface IPingService
{
    public Task<PingResult.PingResult> GetPing();
}
