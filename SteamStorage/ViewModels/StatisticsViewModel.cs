using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Painting;
using SteamStorage.Models;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.ViewModels;

public class StatisticsViewModel : ViewModelBase
{
    #region Fields

    private readonly StatisticsModel _statisticsModel;
    private readonly ChartTooltipModel _chartTooltipModel;

    #endregion Fields

    #region Properties

    public string InvestedSumString
    {
        get => _statisticsModel.InvestedSumString;
    }

    public double InvestedSumGrowth
    {
        get => _statisticsModel.InvestedSumGrowth;
    }

    public IEnumerable<ISeries> InvestedSumGrowthSeries
    {
        get => _statisticsModel.InvestedSumGrowthSeries;
    }
    
    public double InvestedSumWidth
    {
        get => _statisticsModel.InvestedSumWidth;
    }

    public string FinancialGoalString
    {
        get => _statisticsModel.FinancialGoalString;
    }

    public double FinancialGoalPercentageCompletion
    {
        get => _statisticsModel.FinancialGoalPercentageCompletion;
    }

    public IEnumerable<ISeries> FinancialGoalPercentageCompletionSeries
    {
        get => _statisticsModel.FinancialGoalPercentageCompletionSeries;
    }
    
    public double FinancialGoalWidth
    {
        get => _statisticsModel.FinancialGoalWidth;
    }

    public int TotalCount
    {
        get => _statisticsModel.TotalCount;
    }

    public int ActivesCount
    {
        get => _statisticsModel.ActivesCount;
    }

    public string ActivesCurrentSumString
    {
        get => _statisticsModel.ActivesCurrentSumString;
    }

    public double ActivesPercentageGrowth
    {
        get => _statisticsModel.ActivesPercentageGrowth;
    }

    public int ArchivesCount
    {
        get => _statisticsModel.ArchivesCount;
    }

    public string ArchivesSoldSumString
    {
        get => _statisticsModel.ArchivesSoldSumString;
    }

    public double ArchivesPercentageGrowth
    {
        get => _statisticsModel.ArchivesPercentageGrowth;
    }

    public int InventoryCount
    {
        get => _statisticsModel.InventoryCount;
    }

    public string InventorySumString
    {
        get => _statisticsModel.InventorySumString;
    }

    public IEnumerable<ISeries> InventoryGamesSeries
    {
        get => _statisticsModel.InventoryGamesSeries;
    }
    
    public double InventoryGamesWidth
    {
        get => _statisticsModel.InventoryGamesWidth;
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

    #endregion Properties

    #region Commands

    public RelayCommand AttachedToVisualTreeCommand
    {
        get => _statisticsModel.AttachedToVisualTreeCommand;
    }

    #endregion Commands

    #region Constructor

    public StatisticsViewModel(
        StatisticsModel statisticsModel,
        ChartTooltipModel chartTooltipModel)
    {
        _statisticsModel = statisticsModel;
        _chartTooltipModel = chartTooltipModel;

        statisticsModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        chartTooltipModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}
