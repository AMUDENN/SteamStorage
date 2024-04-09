using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Services.DialogService;
using SteamStorage.Utilities;
using SteamStorage.ViewModels;

namespace SteamStorage.Models;

public class ProfileModel : ModelBase
{
    #region Fields

    private readonly UserModel _userModel;
    private readonly CurrenciesModel _currenciesModel;
    private readonly PagesModel _pagesModel;
    private readonly TextConfirmDialogViewModel _textConfirmDialogViewModel;
    private readonly IDialogService _dialogService;

    private string _profileUrl;

    private string? _imageUrl;
    private string? _userName;
    private string? _steamId;

    private string? _role;

    private string? _dateRegistration;

    private string? _defaultFinancialGoal;
    private string? _financialGoal;

    private CurrencyModel? _selectedCurrency;

    private string? _exchangeRate;

    private PageModel? _selectedPage;

    #endregion Fields

    #region Properties

    public string? ImageUrl
    {
        get => _imageUrl;
        private set => SetProperty(ref _imageUrl, value);
    }

    public string? UserName
    {
        get => _userName;
        private set => SetProperty(ref _userName, value);
    }

    public string? SteamId
    {
        get => _steamId;
        private set => SetProperty(ref _steamId, value);
    }

    public string? Role
    {
        get => _role;
        private set => SetProperty(ref _role, value);
    }

    public string? DateRegistration
    {
        get => _dateRegistration;
        private set => SetProperty(ref _dateRegistration, value);
    }

    private string? DefaultFinancialGoal
    {
        get => _defaultFinancialGoal;
        set => SetProperty(ref _defaultFinancialGoal, value);
    }
    
    public string? FinancialGoal
    {
        get => _financialGoal;
        set
        {
            SetProperty(ref _financialGoal, value); 
            SaveFinancialGoal.NotifyCanExecuteChanged();
        }
    }

    public CurrencyModel? SelectedCurrency
    {
        get => _selectedCurrency;
        set
        {
            if (value is not null && value.Id != _userModel.Currency?.Id)
                SetCurrency(value);
            SetProperty(ref _selectedCurrency, value);
        }
    }

    public string? ExchangeRate
    {
        get => _exchangeRate;
        private set => SetProperty(ref _exchangeRate, value);
    }

    public PageModel? SelectedPage
    {
        get => _selectedPage;
        set
        {
            if (value is not null && value.Id != _userModel.StartPage?.Id)
                SetPage(value);
            SetProperty(ref _selectedPage, value);
        }
    }

    #endregion Properties

    #region Commands

    public RelayCommand OpenSteamProfileCommand { get; }
    
    public RelayCommand SaveFinancialGoal { get; }

    public AsyncRelayCommand DeleteProfileCommand { get; }

    public RelayCommand AttachedToVisualTreeCommand { get; }

    #endregion Commands

    #region Constructor

    public ProfileModel(
        UserModel userModel,
        CurrenciesModel currenciesModel,
        PagesModel pagesModel,
        TextConfirmDialogViewModel textConfirmDialogViewModel,
        IDialogService dialogService)
    {
        _userModel = userModel;
        _currenciesModel = currenciesModel;
        _pagesModel = pagesModel;
        _textConfirmDialogViewModel = textConfirmDialogViewModel;
        _dialogService = dialogService;

        userModel.UserChanged += UserChangedHandler;
        userModel.CurrencyChanged += CurrencyChangedHandler;
        userModel.StartPageChanged += StartPageChangedHandler;
        userModel.FinancialGoalChanged += FinancialGoalChangedHandler;

        currenciesModel.CurrenciesLoaded += CurrenciesLoadedHandler;

        pagesModel.PagesLoaded += PagesLoadedHandler;

        _profileUrl = string.Empty;

        OpenSteamProfileCommand = new(DoOpenSteamProfileCommand);
        SaveFinancialGoal = new(DoSaveFinancialGoal, CanExecuteSaveFinancialGoal);
        DeleteProfileCommand = new(DoDeleteProfileCommand);
        AttachedToVisualTreeCommand = new(DoAttachedToVisualTreeCommand);
    }

    #endregion Constructor

    #region Methods

    private void UserChangedHandler(object? sender)
    {
        if (_userModel.User is null)
        {
            UserName = null;
            SteamId = null;
            ImageUrl = null;
            Role = null;
            DateRegistration = null;
            SelectedCurrency = null;
            ExchangeRate = null;
            SelectedPage = null;
            _profileUrl = string.Empty;
            return;
        }

        _profileUrl = _userModel.User.ProfileUrl;

        UserName = _userModel.User.Nickname;
        SteamId = $"SteamID: {_userModel.User.SteamId}";
        ImageUrl = _userModel.User.ImageUrlFull;

        Role = $"Роль: {_userModel.User.Role}";

        DateRegistration =
            $"Дата регистрации: {_userModel.User.DateRegistration.ToString(ProgramConstants.VIEW_DATE_FORMAT)}";
    }

    private void CurrencyChangedHandler(object? sender)
    {
        SelectedCurrency = _currenciesModel.CurrencyModels.FirstOrDefault(x => x.Id == _userModel.Currency?.Id);

        ExchangeRate =
            $"1$ = {_userModel.Currency?.Price ?? 0:N2} {_userModel.CurrencyMark} ({(_userModel.Currency?.DateUpdate ?? DateTime.Now).ToString(ProgramConstants.VIEW_DATE_FORMAT)})";
    }
    
    private void StartPageChangedHandler(object? sender)
    {
        SelectedPage = _pagesModel.PageModels.FirstOrDefault(x => x.Id == _userModel.StartPage?.Id);
    }

    private void FinancialGoalChangedHandler(object? sender)
    {
        SetFinancialGoal();
    }

    private void CurrenciesLoadedHandler(object? sender)
    {
        SelectedCurrency = _currenciesModel.CurrencyModels.FirstOrDefault(x => x.Id == _userModel.Currency?.Id);
    }

    private void PagesLoadedHandler(object? sender)
    {
        SelectedPage = _pagesModel.PageModels.FirstOrDefault(x => x.Id == _userModel.StartPage?.Id);
    }

    private void DoOpenSteamProfileCommand()
    {
        UrlUtility.OpenUrl(_profileUrl);
    }

    private void DoSaveFinancialGoal()
    {
        if (string.IsNullOrWhiteSpace(FinancialGoal))
        {
            _userModel.SetFinancialGoalAsync(null);
        }
        if (decimal.TryParse(FinancialGoal, out decimal financialGoal))
        {
            _userModel.SetFinancialGoalAsync(financialGoal);
        }
    }

    private bool CanExecuteSaveFinancialGoal()
    {
        return FinancialGoal != DefaultFinancialGoal;
    }

    private async Task DoDeleteProfileCommand(CancellationToken cancellationToken)
    {
        _textConfirmDialogViewModel.SetConfirmData(
            "Для подтверждения удаления аккаунта введите слово ПОДТВЕРДИТЬ",
            "ПОДТВЕРДИТЬ");

        bool result = await _dialogService.ShowDialogAsync(_textConfirmDialogViewModel);

        if (!result) return;

        await _userModel.DeleteUserAsync();
    }

    private void DoAttachedToVisualTreeCommand()
    {
        _userModel.UpdateCurrencyInfo();
        _pagesModel.GetPagesAsync();
        _currenciesModel.GetCurrenciesAsync();
    }

    private void SetFinancialGoal()
    {
        DefaultFinancialGoal = $"{_userModel.GoalSum:N2}";
        FinancialGoal = DefaultFinancialGoal;
    }

    private void SetCurrency(CurrencyModel? currencyModel)
    {
        _userModel.SetCurrencyAsync(currencyModel);
    }

    private void SetPage(PageModel? pageModel)
    {
        _userModel.SetPageAsync(pageModel);
    }

    #endregion Methods
}
