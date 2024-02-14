using System.Collections.Generic;
using LiveChartsCore;
using SteamStorage.Models;
using SteamStorage.ViewModels.Tools;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Services.PingResult;

namespace SteamStorage.ViewModels;

public class StatisticsViewModel : ViewModelBase
{
    #region Fields

    private readonly StatisticsModel _statisticsModel;
    private readonly UserModel _userModel;

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

    public int TotalCount
    {
        get => _statisticsModel.TotalCount;
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
    
    public IEnumerable<ISeries> InventoryGamesSeries
    {
        get => _statisticsModel.InventoryGamesSeries;
    }

    public long Ping
    {
        get => _statisticsModel.Ping;
    }

    public PingResult.ServerStatus Status
    {
        get => _statisticsModel.Status;
    }

    public string CurrencyMark
    {
        get => _userModel.CurrencyMark;
    }

    #endregion Properties

    #region Constructor

    public StatisticsViewModel(StatisticsModel statisticsModel, UserModel userModel)
    {
        _statisticsModel = statisticsModel;
        _userModel = userModel;

        statisticsModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);

        userModel.CurrencyChanged += CurrencyChangedHandler;
    }

    #endregion Constructor

    #region Methods

    private void CurrencyChangedHandler(object? sender)
    {
        OnPropertyChanged(nameof(CurrencyMark));
    }

    #endregion Methods
}
