using System.Collections.Generic;
using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.ViewModels;

public class HomeViewModel : ViewModelBase
{
    #region Fields

    private readonly HomeModel _homeModel;

    #endregion Fields

    #region Properties

    public ViewModelBase CurrentViewModel
    {
        get => _homeModel.CurrentViewModel;
    }

    public IEnumerable<SecondaryNavigationModel> SecondaryNavigationOptions
    {
        get => _homeModel.SecondaryNavigationOptions;
    }

    public SecondaryNavigationModel? SelectedSecondaryNavigationModel
    {
        get => _homeModel.SelectedSecondaryNavigationModel;
        set => _homeModel.SelectedSecondaryNavigationModel = value;
    }

    #endregion Properties

    #region Constructor

    public HomeViewModel(
        HomeModel homeModel)
    {
        _homeModel = homeModel;
        homeModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
