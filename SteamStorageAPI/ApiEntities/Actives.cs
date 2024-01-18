namespace SteamStorageAPI.ApiEntities;

public class Actives
{
    public record ActiveResponse(
        int Id,
        Skins.BaseSkinResponse Skin,
        int Count,
        decimal BuyPrice,
        decimal CurrentPrice,
        decimal CurrentSum,
        double Change);

    public record ActivesPagesCountResponse(int Count);

    public record ActivesCountResponse(int Count);
}