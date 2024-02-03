namespace SteamStorageAPI.Services.PingService;

public interface IPingService
{
    public Task<PingResult.PingResult> GetPing(string host);
}
