using System.Collections.Generic;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SteamStorage.Models;
using SteamStorage.Models.Actives;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.ViewModels.Tools.BaseViewModels;

namespace SteamStorage.ViewModels.Actives;

public class ActiveGroupEditViewModel : BaseGroupEditViewModel
{
    #region Fields

    private readonly ActiveGroupEditModel _activeGroupEditModel;
    private readonly ChartTooltipModel _chartTooltipModel;

    #endregion Fields

    #region Properties

    public string? DefaultGoalSum => _activeGroupEditModel.DefaultGoalSum;

    public string? GoalSum
    {
        get => _activeGroupEditModel.GoalSum;
        set => _activeGroupEditModel.GoalSum = value;
    }

    public string DateCreationString => _activeGroupEditModel.DateCreationString;

    public string BuySumString => _activeGroupEditModel.BuySumString;

    public string CountString => _activeGroupEditModel.CountString;

    public string CurrentSumString => _activeGroupEditModel.CurrentSumString;

    public string GoalSumCompletion => _activeGroupEditModel.GoalSumCompletion;

    public decimal ChangePeriod => _activeGroupEditModel.ChangePeriod;

    public string DatePeriod => _activeGroupEditModel.DatePeriod;

    public string? NotFoundText => _activeGroupEditModel.NotFoundText;

    public IEnumerable<ISeries> ChangeSeries => _activeGroupEditModel.ChangeSeries;

    public IEnumerable<Axis> XAxis => _activeGroupEditModel.XAxis;

    public IEnumerable<Axis> YAxis => _activeGroupEditModel.YAxis;

    public IEnumerable<PeriodModel> PeriodModels => _activeGroupEditModel.PeriodModels;

    public PeriodModel? SelectedPeriodModel
    {
        get => _activeGroupEditModel.SelectedPeriodModel;
        set => _activeGroupEditModel.SelectedPeriodModel = value;
    }

    public bool? IsLoading => _activeGroupEditModel.IsLoading;

    public SolidColorPaint TooltipTextPaint => _chartTooltipModel.TooltipTextPaint;

    public SolidColorPaint TooltipBackgroundPaint => _chartTooltipModel.TooltipBackgroundPaint;

    #endregion Properties

    #region Constructor

    public ActiveGroupEditViewModel(
        ActiveGroupEditModel activeGroupEditModel,
        ChartTooltipModel chartTooltipModel) : base(activeGroupEditModel)
    {
        _activeGroupEditModel = activeGroupEditModel;
        _chartTooltipModel = chartTooltipModel;

        chartTooltipModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor

    #region Methods

    public void SetEditGroup(ActiveGroupModel? model)
    {
        _activeGroupEditModel.SetEditGroup(model);
    }

    #endregion Methods
}