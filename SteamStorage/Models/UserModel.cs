using System;
using System.Threading.Tasks;
using SteamStorage.Models.Tools;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.Services.Settings.SettingsService;
using SteamStorage.Utilities;
using SteamStorage.Utilities.Events.Settings;
using SteamStorageAPI.SDK.ApiClient;
using SteamStorageAPI.SDK.ApiEntities;
using SteamStorageAPI.SDK.Utilities.ApiControllers;

namespace SteamStorage.Models;

public class UserModel : ModelBase
{
    #region Events

    public delegate void UserChangedEventHandler(object? sender);

    public event UserChangedEventHandler? UserChanged;

    public delegate void CurrencyChangedEventHandler(object? sender);

    public event CurrencyChangedEventHandler? CurrencyChanged;

    public delegate void StartPageChangedEventHandler(object? sender);

    public event StartPageChangedEventHandler? StartPageChanged;

    public delegate void FinancialGoalChangedEventHandler(object? sender);

    public event FinancialGoalChangedEventHandler? FinancialGoalChanged;

    #endregion Events

    #region Fields

    private readonly IApiClient _apiClient;
    private readonly ISettingsService _settingsService;

    private Users.UserResponse? _user;
    private Currencies.CurrencyResponse? _currency;
    private Pages.PageResponse? _startPage;
    private decimal? _goalSum;

    #endregion Fields

    #region Properties

    public Users.UserResponse? User
    {
        get => _user;
        private set
        {
            if (!SetProperty(ref _user, value)) return;
            OnUserChanged();
            if (value is null) return;
            GetCurrencyAsync();
            GetStartPageAsync();
            GetFinancialGoalAsync();
        }
    }

    public Currencies.CurrencyResponse? Currency
    {
        get => _currency;
        private set
        {
            int? id = _currency?.Id;
            DateTime? dateUpdate = _currency?.DateUpdate;

            SetProperty(ref _currency, value);

            if (id != value?.Id || dateUpdate != value?.DateUpdate)
                OnCurrencyChanged();
        }
    }

    public Pages.PageResponse? StartPage
    {
        get => _startPage;
        private set
        {
            int? id = _startPage?.Id;

            SetProperty(ref _startPage, value);

            if (id != value?.Id)
                OnStartPageChanged();
        }
    }

    public decimal? GoalSum
    {
        get => _goalSum;
        private set
        {
            SetProperty(ref _goalSum, value);
            OnFinancialGoalChanged();
        }
    }

    public string CurrencyMark => Currency?.Mark ?? ProgramConstants.BASE_CURRENCY_MARK;

    #endregion Properties

    #region Constructor

    public UserModel(
        IApiClient apiClient,
        ISettingsService settingsService)
    {
        _apiClient = apiClient;
        _settingsService = settingsService;

        settingsService.SettingsPropertyChanged += SettingsPropertyChangedHandler;

        GetUserAsync();
    }

    #endregion Constructor

    #region Methods

    private void SettingsPropertyChangedHandler(object? sender, SettingsPropertyChangedEventArgs e)
    {
        if (e.Property != nameof(_settingsService.UserSettings.Token)) return;
        GetUserAsync();
    }

    private async void GetUserAsync()
    {
        User = string.IsNullOrWhiteSpace(_settingsService.UserSettings.Token)
            ? null
            : await _apiClient.GetAsync<Users.UserResponse>(
                ApiConstants.ApiMethods.GetCurrentUserInfo);
    }

    private async void GetCurrencyAsync()
    {
        Currency = await _apiClient.GetAsync<Currencies.CurrencyResponse>(
            ApiConstants.ApiMethods.GetCurrentCurrency);
    }

    private async void GetStartPageAsync()
    {
        StartPage = await _apiClient.GetAsync<Pages.PageResponse>(
            ApiConstants.ApiMethods.GetCurrentStartPage);
    }

    private async void GetFinancialGoalAsync()
    {
        Users.GoalSumResponse? response = await _apiClient.GetAsync<Users.GoalSumResponse>(
            ApiConstants.ApiMethods.GetCurrentUserGoalSum);

        GoalSum = response?.GoalSum;
    }

    public void UpdateCurrencyInfo()
    {
        GetCurrencyAsync();
    }

    public async Task DeleteUserAsync()
    {
        await _apiClient.DeleteAsync(ApiConstants.ApiMethods.DeleteUser);
        GetUserAsync();
    }

    public async Task SetCurrencyAsync(CurrencyModel currencyModel)
    {
        await _apiClient.PutAsync(
            ApiConstants.ApiMethods.SetCurrency,
            new Currencies.SetCurrencyRequest(currencyModel.Id));

        GetCurrencyAsync();
    }

    public async Task SetPageAsync(PageModel pageModel)
    {
        await _apiClient.PutAsync(
            ApiConstants.ApiMethods.SetStartPage,
            new Pages.SetPageRequest(pageModel.Id));

        GetStartPageAsync();
    }

    public async Task SetFinancialGoalAsync(decimal? goalSum)
    {
        await _apiClient.PutAsync(
            ApiConstants.ApiMethods.PutGoalSum,
            new Users.PutGoalSumRequest(goalSum));

        GetFinancialGoalAsync();
    }

    private void OnUserChanged()
    {
        UserChanged?.Invoke(this);
    }

    private void OnCurrencyChanged()
    {
        CurrencyChanged?.Invoke(this);
    }

    private void OnStartPageChanged()
    {
        StartPageChanged?.Invoke(this);
    }

    private void OnFinancialGoalChanged()
    {
        FinancialGoalChanged?.Invoke(this);
    }

    #endregion Methods
}