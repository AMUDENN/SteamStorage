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

    public record ActiveGroupResponse(
        int Id,
        string Title,
        string? Description,
        string Colour,
        decimal? GoalSum,
        double? GoalSumCompletion,
        int Count,
        decimal BuySum,
        decimal CurrentSum,
        double Change,
        DateTime DateCreation) : Response;

    public record ActiveGroupsResponse(
        int Count,
        IEnumerable<ActiveGroupResponse>? ActiveGroups) : Response;

    public record ActiveGroupDynamicResponse(
        int Id,
        DateTime DateUpdate,
        decimal Sum) : Response;

    public record ActiveGroupDynamicStatsResponse(
        double ChangePeriod,
        IEnumerable<ActiveGroupDynamicResponse> Dynamic) : Response;

    public record ActiveGroupsCountResponse(
        int Count) : Response;

    public record GetActiveGroupsRequest(
        ActiveGroupOrderName? OrderName,
        bool? IsAscending) : Request;

    public record GetActiveGroupDynamicRequest(
        int GroupId,
        DateTime StartDate,
        DateTime EndDate) : Request;

    public record PostActiveGroupRequest(
        string Title,
        string? Description,
        string? Colour,
        decimal? GoalSum) : Request;
    
    public record PutActiveGroupRequest(
        int GroupId,
        string Title,
        string? Description,
        string? Colour,
        decimal? GoalSum) : Request;
    
    public record DeleteActiveGroupRequest(
        int GroupId) : Request;

    #endregion Records
}
