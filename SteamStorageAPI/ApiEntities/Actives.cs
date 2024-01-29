using SteamStorageAPI.ApiEntities.Tools;
using SteamStorageAPI.Utilities;

namespace SteamStorageAPI.ApiEntities;

public static class Actives
{
    #region Enums

    public enum ActiveOrderName
    {
        Title,
        Count,
        BuyPrice,
        CurrentPrice,
        CurrentSum,
        Change
    }

    #endregion Enums

    #region Records

    public record ActiveResponse(
        int Id,
        Skins.BaseSkinResponse Skin,
        int Count,
        decimal BuyPrice,
        decimal CurrentPrice,
        decimal CurrentSum,
        double Change);

    public record ActivesPagesCountResponse(int Count);

    public record ActivesCountResponse(int Count);

    public record GetActivesRequest(
        int? GroupId,
        int? GameId,
        string? Filter,
        ActiveOrderName? OrderName,
        bool? IsAscending,
        int PageNumber,
        int PageSize) : Request;

    public record GetActivesPagesCountRequest(int? GroupId, int? GameId, string? Filter, int PageSize) : Request;

    public record GetActivesCountRequest(int? GroupId, int? GameId, string? Filter) : Request;

    public record PostActiveRequest(
        int GroupId,
        int Count,
        decimal BuyPrice,
        decimal? GoalPrice,
        int SkinId,
        string? Description,
        DateTime BuyDate) : Request;

    public record PutActiveRequest(
        int Id,
        int GroupId,
        int Count,
        decimal BuyPrice,
        decimal? GoalPrice,
        int SkinId,
        string? Description,
        DateTime BuyDate) : Request;

    public record SoldActiveRequest(
        int Id,
        int GroupId,
        int Count,
        decimal SoldPrice,
        DateTime SoldDate,
        string? Description) : Request;

    public record DeleteActiveRequest(int Id) : Request;

    #endregion Records
}