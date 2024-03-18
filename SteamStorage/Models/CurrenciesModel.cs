using System.Collections.Generic;
using System.Linq;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
using SteamStorageAPI;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Utilities;

namespace SteamStorage.Models;

public class CurrenciesModel : ModelBase
{
    #region Events

    public delegate void CurrenciesLoadedEventHandler(object? sender);

    public event CurrenciesLoadedEventHandler? CurrenciesLoaded;

    #endregion Events

    #region Fields

    private readonly ApiClient _apiClient;

    private List<CurrencyModel> _currencyModels;

    #endregion Fields

    #region Properties

    public List<CurrencyModel> CurrencyModels
    {
        get => _currencyModels;
        private set => SetProperty(ref _currencyModels, value);
    }

    #endregion Properties

    #region Constructor

    public CurrenciesModel(
        ApiClient apiClient)
    {
        _apiClient = apiClient;

        _currencyModels = [];

        GetCurrencies();
    }

    #endregion Constructor

    #region Methods

    private async void GetCurrencies()
    {
        IEnumerable<Currencies.CurrencyResponse>? currencyResponses =
            await _apiClient.GetAsync<IEnumerable<Currencies.CurrencyResponse>>(
                ApiConstants.ApiControllers.Currencies,
                ApiConstants.ApiMethods.GetCurrencies);
        if (currencyResponses is null) return;
        CurrencyModels = currencyResponses.Select(x => new CurrencyModel(x.Id, x.Title, x.Mark)).ToList();
        OnCurrenciesLoaded();
    }

    private void OnCurrenciesLoaded()
    {
        CurrenciesLoaded?.Invoke(this);
    }

    #endregion Methods
}
