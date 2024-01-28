namespace SteamStorageAPI.ApiEntities;

public class Authorize
{
    #region Records

    public record AuthUrlResponse(string Url, string Group);

    public record AuthResponse(string Token);

    #endregion Records
}