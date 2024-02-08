using SteamStorage.Models.Tools;
using SteamStorage.Utilities;
using SteamStorageAPI;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Events;
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

    private Currencies.CurrencyResponse? Currency
    {
        get => _currency;
        set
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

    public UserModel(ApiClient apiClient)
    {
        _apiClient = apiClient;

        apiClient.TokenChanged += TokenChangedHandler;
    }

    #endregion Constructor

    #region Methods

    private async void TokenChangedHandler(object? sender, TokenChangedEventArgs e)
    {
        User = string.IsNullOrEmpty(e.Token)
            ? null
            : await _apiClient.GetAsync<Users.UserResponse>(ApiConstants.ApiControllers.Users,
                "GetCurrentUserInfo");
    }

    private async void GetCurrency()
    {
        Currency = await _apiClient.GetAsync<Currencies.CurrencyResponse, Currencies.GetCurrencyRequest>(
            ApiConstants.ApiControllers.Currencies,
            "GetCurrency", new(User?.CurrencyId ?? ProgramConstants.BASE_CURRENCY_ID));
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
