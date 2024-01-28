using System.Collections.Generic;
using SteamStorage.Models;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Services.PingResult;

namespace SteamStorage.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        #region Fields

        private readonly StatisticsModel _statisticsModel;
        private readonly ListItemsModel _itemsModel;

        #endregion Fields

        #region Properties

        public double InvestedSum
        {
            get => _statisticsModel.InvestedSum;
        }

        public double InvestedSumGrowth
        {
            get => _statisticsModel.InvestedSumGrowth;
        }

        public double FinancialGoal
        {
            get => _statisticsModel.FinancialGoal;
        }

        public double FinancialGoalPercentageCompletion
        {
            get => _statisticsModel.FinancialGoalPercentageCompletion;
        }

        public int ActivesCount
        {
            get => _statisticsModel.ActivesCount;
        }

        public double ActivesCurrentSum
        {
            get => _statisticsModel.ActivesCurrentSum;
        }

        public double ActivesPercentageGrowth
        {
            get => _statisticsModel.ActivesPercentageGrowth;
        }

        public int ArchivesCount
        {
            get => _statisticsModel.ArchivesCount;
        }

        public double ArchivesSoldSum
        {
            get => _statisticsModel.ArchivesSoldSum;
        }

        public double ArchivesPercentageGrowth
        {
            get => _statisticsModel.ArchivesPercentageGrowth;
        }

        public int InventoryCount
        {
            get => _statisticsModel.InventoryCount;
        }

        public double InventorySum
        {
            get => _statisticsModel.InventorySum;
        }

        public IEnumerable<Statistics.InventoryGameStatisticResponse> InventoryGames
        {
            get => _statisticsModel.InventoryGames;
        }

        public long Ping
        {
            get => _statisticsModel.Ping;
        }

        public PingResult.ServerStatus Status
        {
            get => _statisticsModel.Status;
        }

        #endregion Properties

        #region Constructor

        public HomeViewModel(StatisticsModel statisticsModel, ListItemsModel itemsModel)
        {
            _statisticsModel = statisticsModel;
            _itemsModel = itemsModel;

            statisticsModel.PropertyChanged += (s, e) => { OnPropertyChanged(e.PropertyName); };
        }

        #endregion Constructor
    }
}
