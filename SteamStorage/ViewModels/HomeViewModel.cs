using SteamStorage.ViewModels.Tools;

namespace SteamStorage.ViewModels;

public class HomeViewModel : ViewModelBase
{
    #region Properties

    public ViewModelBase StatisticsViewModel { get; }

    public ViewModelBase ListItemsViewModel { get; }

    #endregion Properties

    #region Constructor

    public HomeViewModel(StatisticsViewModel statisticsViewModel, ListItemsViewModel itemsViewModel)
    {
        StatisticsViewModel = statisticsViewModel;
        ListItemsViewModel = itemsViewModel;
    }

    #endregion Constructor
}
