using System.Collections.Generic;
using System.Linq;
using SteamStorage.Models.Tools;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.ViewModels.Home;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.Models.Home;

public class HomeModel : ModelBase
{
    #region Fields

    private ViewModelBase _currentViewModel;

    private SecondaryNavigationModel? _selectedSecondaryNavigationModel;

    #endregion Fields

    #region Properties

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        private set => SetProperty(ref _currentViewModel, value);
    }

    public IEnumerable<SecondaryNavigationModel> SecondaryNavigationOptions { get; }

    public SecondaryNavigationModel? SelectedSecondaryNavigationModel
    {
        get => _selectedSecondaryNavigationModel;
        set
        {
            SetProperty(ref _selectedSecondaryNavigationModel, value);
            if (value is null) return;
            CurrentViewModel = value.Page;
        }
    }

    #endregion Properties

    #region Constructor

    public HomeModel(
        StatisticsViewModel statisticsViewModel,
        ListItemsViewModel listItemsViewModel)
    {
        SecondaryNavigationOptions =
        [
            new("Статистика", statisticsViewModel, true),
            new("Список предметов", listItemsViewModel, true)
        ];

        _selectedSecondaryNavigationModel = SecondaryNavigationOptions.First();
        _currentViewModel = _selectedSecondaryNavigationModel.Page;
    }

    #endregion Constructor
}
