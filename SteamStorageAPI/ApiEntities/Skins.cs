namespace SteamStorageAPI.ApiEntities;

public class Skins
{
    public record BaseSkinResponse(
        int Id,
        string SkinIconUrl,
        string Title,
        string MarketHashName,
        string MarketUrl);

    public record SkinResponse(
        BaseSkinResponse Skin,
        decimal CurrentPrice,
        double Change7D,
        double Change30D,
        bool IsMarked);

    public record SkinDynamicResponse(int Id, DateTime DateUpdate, decimal Price);

    public record SkinPagesCountResponse(int Count);

    public record SteamSkinsCountResponse(int Count);

    public record SavedSkinsCountResponse(int Count);
}