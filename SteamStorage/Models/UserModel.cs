using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Services.Settings.SettingsService;
using SteamStorage.Utilities;
using SteamStorage.Utilities.Events.Settings;
using SteamStorageAPI;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Utilities;

namespace SteamStorage.Models;

public class UserModel : ModelBase
{
    #region Events

    public delegate void UserChangedEventHandler(object? sender);

    public event UserChangedEventHandler? UserChanged;

    public delegate void CurrencyChangedEventHandler(object? sender);

    public event CurrencyChangedEventHandler? CurrencyChanged;

    #endregion Events

    #region Fields

    private readonly ApiClient _apiClient;
    private readonly ISettingsService _settingsService;

    private Users.UserResponse? _user;
    private Currencies.CurrencyResponse? _currency;

    #endregion Fields

    #region Properties

    public Users.UserResponse? User
    {
        get => _user;
        private set
        {
            SetProperty(ref _user, value);
            OnUserChanged();
            GetCurrency();
        }
    }

    public Currencies.CurrencyResponse? Currency
    {
        get => _currency;
        private set
        {
            SetProperty(ref _currency, value);
            OnCurrencyChanged();
        }
    }

    public string CurrencyMark
    {
        get => Currency?.Mark ?? ProgramConstants.BASE_CURRENCY_MARK;
    }

    #endregion Properties

    #region Constructor

    public UserModel(
        ApiClient apiClient,
        ISettingsService settingsService)
    {
        _apiClient = apiClient;
        _settingsService = settingsService;

        settingsService.SettingsPropertyChanged += SettingsPropertyChangedHandler;

        SetUser();
    }

    #endregion Constructor

    #region Methods

    private void SettingsPropertyChangedHandler(object? sender, SettingsPropertyChangedEventArgs e)
    {
        if (e.Property != nameof(_settingsService.UserSettings.Token)) return;
        SetUser();
    }

    private async void SetUser()
    {
        User = string.IsNullOrEmpty(_settingsService.UserSettings.Token)
            ? null
            : await _apiClient.GetAsync<Users.UserResponse>(
                ApiConstants.ApiMethods.GetCurrentUserInfo);
    }

    private async void GetCurrency()
    {
        Currency = await _apiClient.GetAsync<Currencies.CurrencyResponse>(
            ApiConstants.ApiMethods.GetCurrentCurrency);
    }

    public void UpdateCurrencyInfo()
    {
        GetCurrency();
    }

    public async void DeleteUser()
    {
        await _apiClient.DeleteAsync(ApiConstants.ApiMethods.DeleteUser);
        SetUser();
    }

    public async void SetCurrency(CurrencyModel? currencyModel)
    {
        if (currencyModel is null) return;

        await _apiClient.PutAsync(
            ApiConstants.ApiMethods.SetCurrency,
            new Currencies.SetCurrencyRequest(currencyModel.Id));

        GetCurrency();
    }
    
    public async void SetPage(PageModel? pageModel)
    {
        if (pageModel is null) return;
        
        await _apiClient.PutAsync(
            ApiConstants.ApiMethods.SetStartPage,
            new Pages.SetPageRequest(pageModel.Id));
    }

    private void OnUserChanged()
    {
        UserChanged?.Invoke(this);
    }

    private void OnCurrencyChanged()
    {
        CurrencyChanged?.Invoke(this);
    }

    #endregion Methods
}
