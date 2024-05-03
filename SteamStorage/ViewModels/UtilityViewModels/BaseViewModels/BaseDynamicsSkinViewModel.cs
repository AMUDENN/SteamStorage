using System.Collections.Generic;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Models.UtilityModels.BaseModels;

namespace SteamStorage.ViewModels.UtilityViewModels.BaseViewModels;

public class BaseDynamicsSkinViewModel : BaseSkinViewModel
{
    #region Fields

    private readonly BaseDynamicsSkinModel _model;
    private readonly ChartTooltipModel _chartTooltipModel;

    #endregion Fields

    #region Properties

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
    
    public IEnumerable<PeriodModel> PeriodModels
    {
        get => _model.PeriodModels;
    }

    public PeriodModel? SelectedPeriodModel
    {
        get => _model.SelectedPeriodModel;
        set => _model.SelectedPeriodModel = value;
    }
    
    public bool IsLoading
    {
        get => _model.IsLoading;
    }

    #endregion Properties

    #region Constructor

    protected BaseDynamicsSkinViewModel(
        BaseDynamicsSkinModel model,
        ChartTooltipModel chartTooltipModel) : base(model)
    {
        _model = model;
        _chartTooltipModel = chartTooltipModel;

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
