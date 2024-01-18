namespace SteamStorageAPI.ApiEntities;

public class Currencies
{
    public record CurrencyResponse(int Id, int SteamCurrencyId, string Title, string Mark, double Price);
}