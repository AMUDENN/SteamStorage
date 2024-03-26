using SteamStorage.Models.Tools;
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
        Currency = await _apiClient.GetAsync<Currencies.CurrencyResponse, Currencies.GetCurrencyRequest>(
            ApiConstants.ApiMethods.GetCurrency,
            new(User?.CurrencyId ?? ProgramConstants.BASE_CURRENCY_ID));
    }

    public void UpdateCurrencyInfo()
    {
        GetCurrency();
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
