using SteamStorageAPI.ApiEntities.Tools;

namespace SteamStorageAPI.ApiEntities;

public static class Currencies
{
    #region Records

    public record CurrencyResponse(
        int Id,
        int SteamCurrencyId,
        string Title,
        string Mark,
        double Price,
        DateTime DateUpdate) : Response;
    
    public record CurrenciesResponse(
        int Count,
        IEnumerable<CurrencyResponse> Currencies) : Response;

    public record GetCurrencyRequest(
        int Id) : Request;

    public record SetCurrencyRequest(
        int CurrencyId) : Request;

    #endregion Records
}
