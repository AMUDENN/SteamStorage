namespace SteamStorageAPI.ApiEntities;

public class Games
{
    public record GameResponse(int Id, int SteamGameId, string Title, string GameIconUrl);
}