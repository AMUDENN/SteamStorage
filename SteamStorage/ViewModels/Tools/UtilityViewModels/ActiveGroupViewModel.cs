using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SteamStorage.Models;
using SteamStorage.Models.Actives;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.ViewModels.Tools.UtilityViewModels.BaseViewModels;

namespace SteamStorage.ViewModels.Tools.UtilityViewModels;

public class ActiveGroupViewModel : BaseExtendedGroupViewModel
{
    #region Fields

    private readonly ActiveGroupModel _model;
    private readonly ActiveGroupsModel _activeGroupsModel;
    private readonly ChartTooltipModel _chartTooltipModel;

    #endregion Fields

    #region Properties

    public ActiveGroupModel ActiveGroupModel => _model;

    public string GoalSumString => _model.GoalSumString;

    public string BuySumString => _model.BuySumString;

    public string CurrentSumString => _model.CurrentSumString;

    public decimal Change => _model.Change;

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

    #region Commands

    public RelayCommand<ActiveGroupModel> OpenActivesCommand => _activeGroupsModel.OpenActivesCommand;

    public RelayCommand<ActiveGroupModel> AddActiveCommand => _activeGroupsModel.AddActiveCommand;

    public RelayCommand AddActiveGroupCommand => _activeGroupsModel.AddActiveGroupCommand;

    public RelayCommand<ActiveGroupModel> EditActiveGroupCommand => _activeGroupsModel.EditActiveGroupCommand;

    public AsyncRelayCommand<ActiveGroupModel> DeleteActiveGroupCommand => _activeGroupsModel.DeleteActiveGroupCommand;

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