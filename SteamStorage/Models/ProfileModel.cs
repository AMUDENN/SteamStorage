using System;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Utilities;

namespace SteamStorage.Models;

public class ProfileModel : ModelBase
{
    #region Fields

    private readonly UserModel _userModel;
    private readonly CurrenciesModel _currenciesModel;
    private readonly PagesModel _pagesModel;

    private string? _imageUrl;
    private string? _userName;
    private string? _steamId;
    
    private string? _role;
    
    private string? _dateRegistration;

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

    public CurrencyModel? SelectedCurrency
    {
        get => _selectedCurrency;
        set
        {
            SetProperty(ref _selectedCurrency, value); 
            SetCurrency();
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
            SetProperty(ref _selectedPage, value); 
            SetPage();
        }
    }

    #endregion Properties
    
    #region Commands
    
    public RelayCommand OpenSteamProfileCommand { get; }
    
    public RelayCommand DeleteProfileCommand { get; }
    
    public RelayCommand AttachedToVisualTreeCommand { get; }
    
    #endregion Commands

    #region Constructor

    public ProfileModel(
        UserModel userModel,
        CurrenciesModel currenciesModel,
        PagesModel pagesModel)
    {
        _userModel = userModel;
        _currenciesModel = currenciesModel;
        _pagesModel = pagesModel;

        userModel.UserChanged += UserChangedHandler;
        userModel.CurrencyChanged += CurrencyChangedHandler;

        currenciesModel.CurrenciesLoaded += CurrenciesLoadedHandler;
        
        pagesModel.PagesLoaded += PagesLoadedHandler;

        OpenSteamProfileCommand = new(DoOpenSteamProfileCommand);
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
        }

        UserName = _userModel.User?.Nickname;
        SteamId = $"SteamID: {_userModel.User?.SteamId}";
        ImageUrl = _userModel.User?.ImageUrlFull;

        Role = $"Роль: {_userModel.User?.Role}";

        DateRegistration =
            $"Дата регистрации: {_userModel.User?.DateRegistration.ToString(ProgramConstants.VIEW_DATE_FORMAT)}";

        SelectedPage ??= _pagesModel.PageModels.FirstOrDefault(x => x.Id == _userModel.User?.StartPageId);
    }

    private void CurrencyChangedHandler(object? sender)
    {
        SelectedCurrency ??= _currenciesModel.CurrencyModels.FirstOrDefault(x => x.Id == _userModel.Currency?.Id);

        ExchangeRate =
            $"1$ = {_userModel.Currency?.Price ?? 0:N2} {_userModel.CurrencyMark} ({(_userModel.Currency?.DateUpdate ?? DateTime.Now).ToString(ProgramConstants.VIEW_DATE_FORMAT)})";
    }

    private void CurrenciesLoadedHandler(object? sender)
    {
        SelectedCurrency = _currenciesModel.CurrencyModels.FirstOrDefault(x => x.Id == _userModel.Currency?.Id);
    }
    
    private void PagesLoadedHandler(object? sender)
    {
        SelectedPage = _pagesModel.PageModels.FirstOrDefault(x => x.Id == _userModel.User?.StartPageId);
    }

    private void DoOpenSteamProfileCommand()
    {
        
    }

    private void DoDeleteProfileCommand()
    {
        
    }

    private void DoAttachedToVisualTreeCommand()
    {
        _userModel.UpdateCurrencyInfo();
    }

    private void SetCurrency()
    {
        //TODO: SetCurrency API
        _userModel.UpdateCurrencyInfo();
    }

    private void SetPage()
    {
        
    }

    #endregion Methods
}
