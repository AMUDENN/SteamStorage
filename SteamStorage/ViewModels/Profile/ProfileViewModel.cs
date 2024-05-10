using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models;
using SteamStorage.Models.Profile;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.ViewModels.Profile;

public class ProfileViewModel : ViewModelBase
{
    #region Fields

    private readonly ProfileModel _profileModel;
    private readonly CurrenciesModel _currenciesModel;
    private readonly PagesModel _pagesModel;

    #endregion Fields

    #region Properties

    public string? ImageUrl
    {
        get => _profileModel.ImageUrl;
    }

    public string? UserName
    {
        get => _profileModel.UserName;
    }

    public string? SteamId
    {
        get => _profileModel.SteamId;
    }

    public string? Role
    {
        get => _profileModel.Role;
    }

    public string? DateRegistration
    {
        get => _profileModel.DateRegistration;
    }

    public string? FinancialGoal
    {
        get => _profileModel.FinancialGoal;
        set => _profileModel.FinancialGoal = value;
    }

    public IEnumerable<CurrencyModel> CurrencyModels
    {
        get => _currenciesModel.CurrencyModels;
    }

    public CurrencyModel? SelectedCurrency
    {
        get => _profileModel.SelectedCurrency;
        set => _profileModel.SelectedCurrency = value;
    }

    public string? ExchangeRate
    {
        get => _profileModel.ExchangeRate;
    }

    public IEnumerable<PageModel> PageModels
    {
        get => _pagesModel.PageModels;
    }

    public PageModel? SelectedPage
    {
        get => _profileModel.SelectedPage;
        set => _profileModel.SelectedPage = value;
    }

    #endregion Properties

    #region Commands

    public RelayCommand OpenSteamProfileCommand
    {
        get => _profileModel.OpenSteamProfileCommand;
    }

    public AsyncRelayCommand SaveFinancialGoal
    {
        get => _profileModel.SaveFinancialGoal;
    }
    
    public AsyncRelayCommand DeleteProfileCommand
    {
        get => _profileModel.DeleteProfileCommand;
    }

    public RelayCommand AttachedToVisualTreeCommand
    {
        get => _profileModel.AttachedToVisualTreeCommand;
    }

    #endregion Commands

    #region Constructor

    public ProfileViewModel(
        ProfileModel profileModel,
        CurrenciesModel currenciesModel,
        PagesModel pagesModel)
    {
        _profileModel = profileModel;
        _currenciesModel = currenciesModel;
        _pagesModel = pagesModel;
        
        profileModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        currenciesModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        pagesModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
