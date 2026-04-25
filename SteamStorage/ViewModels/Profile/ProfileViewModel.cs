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

    #endregion Fields

    #region Properties

    public string? ImageUrl => _profileModel.ImageUrl;

    public string? UserName => _profileModel.UserName;

    public string? SteamId => _profileModel.SteamId;

    public string? Role => _profileModel.Role;

    public string? DateRegistration => _profileModel.DateRegistration;

    public string? FinancialGoal
    {
        get => _profileModel.FinancialGoal;
        set => _profileModel.FinancialGoal = value;
    }

    public IEnumerable<CurrencyModel> CurrencyModels => _currenciesModel.CurrencyModels;

    public CurrencyModel? SelectedCurrency
    {
        get => _profileModel.SelectedCurrency;
        set => _profileModel.SelectedCurrency = value;
    }

    public string? ExchangeRate => _profileModel.ExchangeRate;

    #endregion Properties

    #region Commands

    public RelayCommand OpenSteamProfileCommand => _profileModel.OpenSteamProfileCommand;

    public AsyncRelayCommand SaveFinancialGoal => _profileModel.SaveFinancialGoal;

    public AsyncRelayCommand DeleteProfileCommand => _profileModel.DeleteProfileCommand;

    public RelayCommand AttachedToVisualTreeCommand => _profileModel.AttachedToVisualTreeCommand;

    #endregion Commands

    #region Constructor

    public ProfileViewModel(
        ProfileModel profileModel,
        CurrenciesModel currenciesModel)
    {
        _profileModel = profileModel;
        _currenciesModel = currenciesModel;

        profileModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        currenciesModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}