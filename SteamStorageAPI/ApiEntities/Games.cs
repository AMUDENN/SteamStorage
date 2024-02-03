using SteamStorageAPI.ApiEntities.Tools;

namespace SteamStorageAPI.ApiEntities;

public static class Games
{
    #region Records

    public record GameResponse(
        int Id,
        int SteamGameId,
        string Title,
        string GameIconUrl) : Response;

    #endregion Records
}
