using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SteamStorage.Models;
using SteamStorage.Models.UtilityModels;
using SteamStorage.ViewModels.UtilityViewModels.BaseViewModels;

namespace SteamStorage.ViewModels.UtilityViewModels;

public class ActiveGroupViewModel : BaseExtendedGroupViewModel
{
    #region Fields

    private readonly ActiveGroupModel _model;
    private readonly ActiveGroupsModel _activeGroupsModel;
    private readonly ChartTooltipModel _chartTooltipModel;

    #endregion Fields

    #region Properties

    public ActiveGroupModel ActiveGroupModel
    {
        get => _model;
    }

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

    #region Commands

    public RelayCommand<ActiveGroupModel> OpenActivesCommand
    {
        get => _activeGroupsModel.OpenActivesCommand;
    }

    public RelayCommand<ActiveGroupModel> AddActiveCommand
    {
        get => _activeGroupsModel.AddActiveCommand;
    }

    public RelayCommand AddActiveGroupCommand
    {
        get => _activeGroupsModel.AddActiveGroupCommand;
    }

    public RelayCommand<ActiveGroupModel> EditActiveGroupCommand
    {
        get => _activeGroupsModel.EditActiveGroupCommand;
    }

    public AsyncRelayCommand<ActiveGroupModel> DeleteActiveGroupCommand
    {
        get => _activeGroupsModel.DeleteActiveGroupCommand;
    }

    #endregion Commands

    #region Constructor

    public ActiveGroupViewModel(
        ActiveGroupModel model,
        ActiveGroupsModel activeGroupsModel,
        ChartTooltipModel chartTooltipModel) : base(model)
    {
        _model = model;
        _activeGroupsModel = activeGroupsModel;
        _chartTooltipModel = chartTooltipModel;

        activeGroupsModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
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
