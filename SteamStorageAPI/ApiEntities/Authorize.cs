namespace SteamStorageAPI.ApiEntities;

public class Authorize
{
    public record AuthUrlResponse(string Url);

    public record AuthResponse(string Token);
}