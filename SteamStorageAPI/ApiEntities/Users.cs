using SteamStorageAPI.ApiEntities.Tools;

namespace SteamStorageAPI.ApiEntities;

public static class Users
{
    #region Records

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
        decimal? GoalSum) : Response;

    public record PutGoalSumRequest(
        decimal? GoalSum) : Request;

    public record PutStartPageRequest(
        int StartPageId) : Request;

    #endregion Records
}
