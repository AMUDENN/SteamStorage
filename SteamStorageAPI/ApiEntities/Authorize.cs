namespace SteamStorageAPI.ApiEntities;

public class Authorize
{
    public record AuthUrlResponse(string Url, string Group);

    public record AuthResponse(string Token);
}