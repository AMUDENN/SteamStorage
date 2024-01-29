using SteamStorageAPI.ApiEntities.Tools;

namespace SteamStorageAPI.ApiEntities;

public static class Inventory
{
    #region Enums

    public enum InventoryOrderName
    {
        Title,
        Count,
        Price,
        Sum
    }

    #endregion Enums

    #region Records

    public record InventoryResponse(int Id, Skins.BaseSkinResponse Skin, int Count);

    public record InventoryPagesCountResponse(int Count);

    public record SavedInventoriesCountResponse(int Count);

    public record GetInventoryRequest(
        int? GameId,
        string? Filter,
        InventoryOrderName? OrderName,
        bool? IsAscending,
        int PageNumber,
        int PageSize) : Request;

    public record GetInventoryPagesCountRequest(int? GameId, string? Filter, int PageSize) : Request;

    public record GetSavedInventoriesCountRequest(int? GameId, string? Filter) : Request;

    public record RefreshInventoryRequest(int GameId) : Request;

    #endregion Records
}