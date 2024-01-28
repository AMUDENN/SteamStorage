using SteamStorageAPI.Utilities;

namespace SteamStorageAPI.ApiEntities;

public static class Currencies
{
    #region Records

    public record CurrencyResponse(int Id, int SteamCurrencyId, string Title, string Mark, double Price);

    public record GetCurrencyRequest(int Id) : Request;

    public record SetCurrencyRequest(int CurrencyId) : Request;

    #endregion Records
}