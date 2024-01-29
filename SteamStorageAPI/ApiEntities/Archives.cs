using SteamStorageAPI.ApiEntities.Tools;

namespace SteamStorageAPI.ApiEntities;

public static class Archives
{
    #region Enums

    public enum ArchiveOrderName
    {
        Title,
        Count,
        BuyPrice,
        SoldPrice,
        SoldSum,
        Change
    }

    #endregion Enums

    #region Records

    public record ArchiveResponse(
        int Id,
        Skins.BaseSkinResponse Skin,
        int Count,
        decimal BuyPrice,
        decimal SoldPrice,
        decimal SoldSum,
        double Change);

    public record ArchivesPagesCountResponse(int Count);

    public record ArchivesCountResponse(int Count);

    public record GetArchivesRequest(
        int? GroupId,
        int? GameId,
        string? Filter,
        ArchiveOrderName? OrderName,
        bool? IsAscending,
        int PageNumber,
        int PageSize) : Request;

    public record GetArchivesPagesCountRequest(int? GroupId, int? GameId, string? Filter, int PageSize) : Request;

    public record GetArchivesCountRequest(int? GroupId, int? GameId, string? Filter) : Request;

    public record PostArchiveRequest(
        int GroupId,
        int Count,
        decimal BuyPrice,
        decimal SoldPrice,
        int SkinId,
        string? Description,
        DateTime BuyDate,
        DateTime SoldDate) : Request;

    public record PutArchiveRequest(
        int Id,
        int GroupId,
        int Count,
        decimal BuyPrice,
        decimal SoldPrice,
        int SkinId,
        string? Description,
        DateTime BuyDate,
        DateTime SoldDate) : Request;

    public record DeleteArchiveRequest(int Id) : Request;

    #endregion Records
}