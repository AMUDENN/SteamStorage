using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Painting;
using SteamStorage.Models;
using SteamStorage.Models.Home;
using SteamStorage.ViewModels.Tools;

namespace SteamStorage.ViewModels.Home;

public class StatisticsViewModel : ViewModelBase
{
    #region Fields

    private readonly StatisticsModel _statisticsModel;
    private readonly ChartTooltipModel _chartTooltipModel;

    #endregion Fields

    #region Properties

    public string InvestedSumString => _statisticsModel.InvestedSumString;

    public decimal InvestedSumGrowth => _statisticsModel.InvestedSumGrowth;

    public IEnumerable<ISeries> InvestedSumGrowthSeries => _statisticsModel.InvestedSumGrowthSeries;

    public double InvestedSumWidth => _statisticsModel.InvestedSumWidth;

    public string FinancialGoalString => _statisticsModel.FinancialGoalString;

    public decimal FinancialGoalPercentageCompletion => _statisticsModel.FinancialGoalPercentageCompletion;

    public IEnumerable<ISeries> FinancialGoalPercentageCompletionSeries => _statisticsModel.FinancialGoalPercentageCompletionSeries;

    public double FinancialGoalWidth => _statisticsModel.FinancialGoalWidth;

    public int TotalCount => _statisticsModel.TotalCount;

    public int ActivesCount => _statisticsModel.ActivesCount;

    public string ActivesCurrentSumString => _statisticsModel.ActivesCurrentSumString;

    public decimal ActivesPercentageGrowth => _statisticsModel.ActivesPercentageGrowth;

    public int ArchivesCount => _statisticsModel.ArchivesCount;

    public string ArchivesSoldSumString => _statisticsModel.ArchivesSoldSumString;

    public decimal ArchivesPercentageGrowth => _statisticsModel.ArchivesPercentageGrowth;

    public int InventoryCount => _statisticsModel.InventoryCount;

    public string InventorySumString => _statisticsModel.InventorySumString;

    public IEnumerable<ISeries> InventoryGamesSeries => _statisticsModel.InventoryGamesSeries;

    public double InventoryGamesWidth => _statisticsModel.InventoryGamesWidth;

    public SolidColorPaint TooltipTextPaint => _chartTooltipModel.TooltipTextPaint;

    public SolidColorPaint TooltipBackgroundPaint => _chartTooltipModel.TooltipBackgroundPaint;

    public long Ping => _statisticsModel.Ping;

    public string ServerStatusString => _statisticsModel.ServerStatusString;

    public bool? ServerStatusBool => _statisticsModel.ServerStatusBool;

    public bool IsServerActive => _statisticsModel.IsServerActive;

    #endregion Properties

    #region Commands

    public RelayCommand AttachedToVisualTreeCommand => _statisticsModel.AttachedToVisualTreeCommand;

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