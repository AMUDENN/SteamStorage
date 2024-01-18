namespace SteamStorageAPI.ApiEntities;

public class Users
{
    public record UserResponse(
        int UserId,
        long SteamId,
        string? ImageUrl,
        string? ImageUrlMedium,
        string? ImageUrlFull,
        string? Nickname,
        int RoleId,
        int StartPageId,
        int CurrencyId,
        DateTime DateRegistration,
        decimal? GoalSum);
}