using SteamStorageAPI.ApiEntities.Tools;

namespace SteamStorageAPI.ApiEntities;

public static class Authorize
{
    #region Records

    public record AuthUrlResponse(
        string Url, 
        string Group) : Response;

    public record AuthResponse(
        string Token) : Response;

    #endregion Records
}