using SteamStorageAPI.ApiEntities.Tools;

namespace SteamStorageAPI.ApiEntities;

public static class ActiveGroups
{
    #region Enums

    public enum ActiveGroupOrderName
    {
        Title,
        Count,
        BuySum,
        CurrentSum,
        Change
    }

    #endregion Enums

    #region Records

    public record ActiveGroupsResponse(
        int Id,
        string Title,
        string Description,
        string Colour,
        decimal? GoalSum) : Response;

    public record ActiveGroupDynamicsResponse(
        int Id,
        DateTime DateUpdate,
        decimal Sum) : Response;

    public record ActiveGroupsCountResponse(
        int Count) : Response;

    public record GetActiveGroupsRequest(
        ActiveGroupOrderName? OrderName,
        bool? IsAscending) : Request;

    public record GetActiveGroupDynamicRequest(
        int GroupId,
        int DaysDynamic) : Request;

    public record PostActiveGroupRequest(
        string Title,
        string? Description,
        string? Colour,
        decimal? GoalSum) : Request;

    #endregion Records
}
