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

    public record SkinDynamicResponse(
        int Id, 
        DateTime DateUpdate, 
        decimal Price) : Response;

    public record SkinPagesCountResponse(
        int Count) : Response;

    public record GetSkinRequest(
        int SkinId) : Request;

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
        int Days) : Request;

    public record GetSkinPagesCountRequest(
        int? GameId, 
        string? Filter, 
        bool? IsMarked, 
        int PageSize) : Request;

    #endregion Records
}