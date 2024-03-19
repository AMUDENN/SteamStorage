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
        int GroupId,
        Skins.BaseSkinResponse Skin,
        DateTime BuyDate,
        DateTime SoldDate,
        int Count,
        decimal BuyPrice,
        decimal SoldPrice,
        decimal SoldSum,
        double Change,
        string? Description) : Response;

    public record ArchivesResponse(
        int Count,
        int PagesCount,
        IEnumerable<ArchiveResponse> Archives) : Response;

    public record ArchivesPagesCountResponse(
        int Count) : Response;

    public record ArchivesCountResponse(
        int Count) : Response;

    public record GetArchivesRequest(
        int? GroupId,
        int? GameId,
        string? Filter,
        ArchiveOrderName? OrderName,
        bool? IsAscending,
        int PageNumber,
        int PageSize) : Request;

    public record GetArchivesPagesCountRequest(
        int? GroupId,
        int? GameId,
        string? Filter,
        int PageSize) : Request;

    public record GetArchivesCountRequest(
        int? GroupId,
        int? GameId,
        string? Filter) : Request;

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

    public record DeleteArchiveRequest(
        int Id) : Request;

    #endregion Records
}
