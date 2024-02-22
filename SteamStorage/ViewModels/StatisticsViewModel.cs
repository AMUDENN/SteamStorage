using System.Collections.Generic;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Painting;
using SteamStorage.Models;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.ViewModels;

public class StatisticsViewModel : ViewModelBase
{
    #region Fields

    private readonly StatisticsModel _statisticsModel;
    private readonly UserModel _userModel;
    private readonly ChartTooltipModel _chartTooltipModel;

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
    
    public IEnumerable<ISeries> InvestedSumGrowthSeries
    {
        get => _statisticsModel.InvestedSumGrowthSeries;
    }

    public double FinancialGoal
    {
        get => _statisticsModel.FinancialGoal;
    }

    public double FinancialGoalPercentageCompletion
    {
        get => _statisticsModel.FinancialGoalPercentageCompletion;
    }
    
    public IEnumerable<ISeries> FinancialGoalPercentageCompletionSeries
    {
        get => _statisticsModel.FinancialGoalPercentageCompletionSeries;
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
    
    public SolidColorPaint TooltipTextPaint
    {
        get => _chartTooltipModel.TooltipTextPaint;
    }
    
    public SolidColorPaint TooltipBackgroundPaint
    {
        get => _chartTooltipModel.TooltipBackgroundPaint;
    }

    public long Ping
    {
        get => _statisticsModel.Ping;
    }
    
    public string ServerStatusString
    {
        get => _statisticsModel.ServerStatusString;
    }
    
    public bool? ServerStatusBool
    {
        get => _statisticsModel.ServerStatusBool;
    }
    
    public bool IsServerActive
    {
        get => _statisticsModel.IsServerActive;
    }

    public string CurrencyMark
    {
        get => _userModel.CurrencyMark;
    }

    #endregion Properties

    #region Constructor

    public StatisticsViewModel(StatisticsModel statisticsModel, ChartTooltipModel chartTooltipModel, UserModel userModel)
    {
        _statisticsModel = statisticsModel;
        _userModel = userModel;
        _chartTooltipModel = chartTooltipModel;

        statisticsModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        chartTooltipModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);

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
