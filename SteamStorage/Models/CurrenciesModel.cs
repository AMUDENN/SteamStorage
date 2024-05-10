using System.Collections.Generic;
using System.Linq;
using SteamStorage.Models.Tools;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorageAPI.SDK;
using SteamStorageAPI.SDK.ApiEntities;
using SteamStorageAPI.SDK.Utilities;

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

        GetCurrenciesAsync();
    }

    #endregion Constructor

    #region Methods

    public async void GetCurrenciesAsync()
    {
        Currencies.CurrenciesResponse? currencyResponses =
            await _apiClient.GetAsync<Currencies.CurrenciesResponse>(
                ApiConstants.ApiMethods.GetCurrencies);
        if (currencyResponses?.Currencies is null) return;
        CurrencyModels = currencyResponses.Currencies.Select(x => new CurrencyModel(x.Id, x.Title, x.Mark)).ToList();
        OnCurrenciesLoaded();
    }

    private void OnCurrenciesLoaded()
    {
        CurrenciesLoaded?.Invoke(this);
    }

    #endregion Methods
}
