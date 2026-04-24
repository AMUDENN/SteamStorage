using System.Collections.Generic;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SteamStorage.Models;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.Models.Tools.UtilityModels.BaseModels;

namespace SteamStorage.ViewModels.Tools.UtilityViewModels.BaseViewModels;

public class BaseDynamicsSkinViewModel : BaseSkinViewModel
{
    #region Fields

    private readonly BaseDynamicsSkinModel _model;
    private readonly ChartTooltipModel _chartTooltipModel;

    #endregion Fields

    #region Properties

    public decimal? ChangePeriod => _model.ChangePeriod;

    public string? DatePeriod => _model.DatePeriod;

    public string? NotFoundText => _model.NotFoundText;

    public IEnumerable<ISeries> ChangeSeries => _model.ChangeSeries;

    public IEnumerable<Axis> XAxis => _model.XAxis;

    public IEnumerable<Axis> YAxis => _model.YAxis;

    public SolidColorPaint TooltipTextPaint => _chartTooltipModel.TooltipTextPaint;

    public SolidColorPaint TooltipBackgroundPaint => _chartTooltipModel.TooltipBackgroundPaint;

    public IEnumerable<PeriodModel> PeriodModels => _model.PeriodModels;

    public PeriodModel? SelectedPeriodModel
    {
        get => _model.SelectedPeriodModel;
        set => _model.SelectedPeriodModel = value;
    }

    public bool IsLoading => _model.IsLoading;

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