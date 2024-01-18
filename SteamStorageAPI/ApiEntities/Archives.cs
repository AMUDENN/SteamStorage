namespace SteamStorageAPI.ApiEntities;

public class Archives
{
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
}