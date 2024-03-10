using System.Collections.Generic;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;

namespace SteamStorage.ViewModels.UtilityViewModels;

public class ActiveGroupViewModel : BaseGroupViewModel
{
    #region Fields

    private readonly ActiveGroupModel _model;
    private readonly ChartTooltipModel _chartTooltipModel;

    #endregion Fields

    #region Properties

    public string GoalSumString
    {
        get => _model.GoalSumString;
    }
    
    public string BuySumString
    {
        get => _model.BuySumString;
    }

    public string CurrentSumString
    {
        get => _model.CurrentSumString;
    }

    public double Change
    {
        get => _model.Change;
    }

    public double? ChangePeriod
    {
        get => _model.ChangePeriod;
    }

    public string? DatePeriod
    {
        get => _model.DatePeriod;
    }

    public string? NotFoundText
    {
        get => _model.NotFoundText;
    }

    public IEnumerable<ISeries> ChangeSeries
    {
        get => _model.ChangeSeries;
    }

    public IEnumerable<Axis> XAxis
    {
        get => _model.XAxis;
    }

    public IEnumerable<Axis> YAxis
    {
        get => _model.YAxis;
    }

    public SolidColorPaint TooltipTextPaint
    {
        get => _chartTooltipModel.TooltipTextPaint;
    }

    public SolidColorPaint TooltipBackgroundPaint
    {
        get => _chartTooltipModel.TooltipBackgroundPaint;
    }

    public bool IsOneDayChecked
    {
        get => _model.IsOneDayChecked;
        set => _model.IsOneDayChecked = value;
    }

    public bool IsOneWeekChecked
    {
        get => _model.IsOneWeekChecked;
        set => _model.IsOneWeekChecked = value;
    }

    public bool IsOneMonthChecked
    {
        get => _model.IsOneMonthChecked;
        set => _model.IsOneMonthChecked = value;
    }

    public bool IsOneYearChecked
    {
        get => _model.IsOneYearChecked;
        set => _model.IsOneYearChecked = value;
    }

    public bool IsLoading
    {
        get => _model.IsLoading;
    }

    #endregion Properties

    #region Constructor

    public ActiveGroupViewModel(
        ActiveGroupModel model,
        ChartTooltipModel chartTooltipModel) : base(model)
    {
        _model = model;
        _chartTooltipModel = chartTooltipModel;

        model.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        chartTooltipModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor

    #region Methods

    public void UpdateStats()
    {
        _model.UpdateStats();
    }

    #endregion Methods
}
