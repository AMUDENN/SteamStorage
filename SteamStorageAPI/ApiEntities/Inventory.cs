﻿using SteamStorageAPI.ApiEntities.Tools;

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

    public record InventoryResponse(
        int Id,
        Skins.BaseSkinResponse Skin,
        int Count,
        decimal CurrentPrice,
        decimal CurrentSum) : Response;

    public record InventoriesResponse(
        int Count,
        int PagesCount,
        IEnumerable<InventoryResponse>? Inventories) : Response;

    public record InventoryPagesCountResponse(
        int Count) : Response;

    public record SavedInventoriesCountResponse(
        int Count) : Response;

    public record GetInventoryRequest(
        int? GameId,
        string? Filter,
        InventoryOrderName? OrderName,
        bool? IsAscending,
        int PageNumber,
        int PageSize) : Request;

    public record GetInventoryPagesCountRequest(
        int? GameId,
        string? Filter,
        int PageSize) : Request;

    public record GetSavedInventoriesCountRequest(
        int? GameId,
        string? Filter) : Request;

    public record RefreshInventoryRequest(
        int GameId) : Request;

    #endregion Records
}
