using SteamStorageAPI.ApiEntities.Tools;

namespace SteamStorageAPI.ApiEntities;

public static class Skins
{
    #region Enums

    public enum SkinOrderName
    {
        Title,
        Price,
        Change7D,
        Change30D
    }

    #endregion Enums

    #region Records

    public record BaseSkinResponse(
        int Id,
        string SkinIconUrl,
        string Title,
        string MarketHashName,
        string MarketUrl) : Response;

    public record SkinResponse(
        BaseSkinResponse Skin,
        decimal CurrentPrice,
        double Change7D,
        double Change30D,
        bool IsMarked) : Response;

    public record SkinsResponse(
        int Count,
        int PagesCount,
        IEnumerable<SkinResponse> Skins) : Response;

    public record SkinDynamicResponse(
        int Id,
        DateTime DateUpdate,
        decimal Price) : Response;

    public record SkinDynamicStatsResponse(
        double ChangePeriod,
        IEnumerable<SkinDynamicResponse> Dynamic) : Response;

    public record SkinPagesCountResponse(
        int Count) : Response;

    public record SavedSkinsCountResponse(
        int Count) : Response;

    public record GetSkinRequest(
        int SkinId) : Request;
    
    public record GetBaseSkinsRequest(
        string? Filter) : Request;

    public record GetSkinsRequest(
        int? GameId,
        string? Filter,
        SkinOrderName? OrderName,
        bool? IsAscending,
        bool? IsMarked,
        int PageNumber,
        int PageSize) : Request;

    public record GetSkinDynamicsRequest(
        int SkinId,
        DateTime StartDate,
        DateTime EndDate) : Request;

    public record GetSkinPagesCountRequest(
        int? GameId,
        string? Filter,
        bool? IsMarked,
        int PageSize) : Request;

    public record GetSavedSkinsCountRequest(
        int? GameId,
        string? Filter,
        bool? IsMarked) : Request;

    public record SetMarkedSkinRequest(
        int SkinId) : Request;

    public record DeleteMarkedSkinRequest(
        int SkinId) : Request;

    #endregion Records
}
