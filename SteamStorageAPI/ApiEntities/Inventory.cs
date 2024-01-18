namespace SteamStorageAPI.ApiEntities;

public class Inventory
{
    public record InventoryResponse(int Id, Skins.BaseSkinResponse Skin, int Count);

    public record InventoryPagesCountResponse(int Count);

    public record SavedInventoriesCountResponse(int Count);
}