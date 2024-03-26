using System.Collections.Generic;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;
using SteamStorage.ViewModels.BaseViewModels;

namespace SteamStorage.ViewModels;

public class ActiveGroupEditViewModel : BaseEditViewModel
{
    #region Fields

    private readonly ActiveGroupEditModel _activeGroupEditModel;
    private readonly ChartTooltipModel _chartTooltipModel;

    #endregion Fields

    #region Properties

    public string DefaultGroupTitle
    {
        get => _activeGroupEditModel.DefaultGroupTitle;
    }

    public string GroupTitle
    {
        get => _activeGroupEditModel.GroupTitle;
        set => _activeGroupEditModel.GroupTitle = value;
    }

    public string? DefaultDescription
    {
        get => _activeGroupEditModel.DefaultDescription;
    }

    public string? Description
    {
        get => _activeGroupEditModel.Description;
        set => _activeGroupEditModel.Description = value;
    }

    public string? DefaultGoalSum
    {
        get => _activeGroupEditModel.DefaultGoalSum;
    }

    public string? GoalSum
    {
        get => _activeGroupEditModel.GoalSum;
        set => _activeGroupEditModel.GoalSum = value;
    }

    public string? DefaultColour
    {
        get => _activeGroupEditModel.DefaultColour;
    }

    public string? Colour
    {
        get => _activeGroupEditModel.Colour;
        set => _activeGroupEditModel.Colour = value;
    }

    public bool IsNewGroup
    {
        get => _activeGroupEditModel.IsNewGroup;
    }

    public string DateCreationString
    {
        get => _activeGroupEditModel.DateCreationString;
    }

    public string BuySumString
    {
        get => _activeGroupEditModel.BuySumString;
    }

    public string CountString
    {
        get => _activeGroupEditModel.CountString;
    }

    public string CurrentSumString
    {
        get => _activeGroupEditModel.CurrentSumString;
    }

    public string GoalSumCompletion
    {
        get => _activeGroupEditModel.GoalSumCompletion;
    }

    public double ChangePeriod
    {
        get => _activeGroupEditModel.ChangePeriod;
    }

    public string DatePeriod
    {
        get => _activeGroupEditModel.DatePeriod;
    }

    public string? NotFoundText
    {
        get => _activeGroupEditModel.NotFoundText;
    }

    public IEnumerable<ISeries> ChangeSeries
    {
        get => _activeGroupEditModel.ChangeSeries;
    }

    public IEnumerable<Axis> XAxis
    {
        get => _activeGroupEditModel.XAxis;
    }

    public IEnumerable<Axis> YAxis
    {
        get => _activeGroupEditModel.YAxis;
    }

    public bool IsOneDayChecked
    {
        get => _activeGroupEditModel.IsOneDayChecked;
        set => _activeGroupEditModel.IsOneDayChecked = value;
    }

    public bool IsOneWeekChecked
    {
        get => _activeGroupEditModel.IsOneWeekChecked;
        set => _activeGroupEditModel.IsOneWeekChecked = value;
    }

    public bool IsOneMonthChecked
    {
        get => _activeGroupEditModel.IsOneMonthChecked;
        set => _activeGroupEditModel.IsOneMonthChecked = value;
    }

    public bool IsOneYearChecked
    {
        get => _activeGroupEditModel.IsOneYearChecked;
        set => _activeGroupEditModel.IsOneYearChecked = value;
    }

    public bool? IsLoading
    {
        get => _activeGroupEditModel.IsLoading;
    }

    public SolidColorPaint TooltipTextPaint
    {
        get => _chartTooltipModel.TooltipTextPaint;
    }

    public SolidColorPaint TooltipBackgroundPaint
    {
        get => _chartTooltipModel.TooltipBackgroundPaint;
    }

    #endregion Properties

    #region Constructor

    public ActiveGroupEditViewModel(
        ActiveGroupEditModel activeGroupEditModel,
        ChartTooltipModel chartTooltipModel) : base(activeGroupEditModel)
    {
        _activeGroupEditModel = activeGroupEditModel;
        _chartTooltipModel = chartTooltipModel;

        activeGroupEditModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
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
