using SteamStorageAPI.ApiEntities.Tools;

namespace SteamStorageAPI.ApiEntities;

public static class ArchiveGroups
{
    #region Enums

    public enum ArchiveGroupOrderName
    {
        Title,
        Count,
        BuySum,
        SoldSum,
        Change
    }

    #endregion Enums

    #region Records

    public record ArchiveGroupsResponse(int Id, string Title, string Description, string Colour);

    public record ArchiveGroupsCountResponse(int Count);

    public record GetArchiveGroupsRequest(ArchiveGroupOrderName? OrderName, bool? IsAscending) : Request;

    public record PostArchiveGroupRequest(string Title, string? Description, string? Colour) : Request;

    public record PutArchiveGroupRequest(int GroupId, string Title, string? Description, string? Colour) : Request;

    public record DeleteArchiveGroupRequest(int GroupId) : Request;

    #endregion Records

}