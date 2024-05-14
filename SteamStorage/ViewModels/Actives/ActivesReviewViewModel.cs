using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Painting;
using SteamStorage.Models;
using SteamStorage.Models.Actives;
using SteamStorage.ViewModels.Tools;
using SteamStorage.ViewModels.Tools.UtilityViewModels;

namespace SteamStorage.ViewModels.Actives;

public class ActivesReviewViewModel : ViewModelBase
{
    #region Fields

    private readonly ActivesReviewModel _activesReviewModel;
    private readonly ActiveGroupsModel _activeGroupsModel;
    private readonly ChartTooltipModel _chartTooltipModel;

    #endregion Fields

    #region Properties

    public int Count
    {
        get => _activesReviewModel.Count;
    }

    public string InvestedSumString
    {
        get => _activesReviewModel.InvestedSumString;
    }

    public string CurrentSumString
    {
        get => _activesReviewModel.CurrentSumString;
    }
    
    public ObservableCollection<ISeries> ActiveGroupsGameCountSeries
    {
        get => _activesReviewModel.ActiveGroupsGameCountSeries;
    }

    public ObservableCollection<ISeries> ActiveGroupsGameInvestmentSumSeries
    {
        get => _activesReviewModel.ActiveGroupsGameInvestmentSumSeries;
    }
    
    public ObservableCollection<ISeries> ActiveGroupsGameCurrentSumSeries
    {
        get => _activesReviewModel.ActiveGroupsGameCurrentSumSeries;
    }
    
    public double ChartMinWidth
    {
        get => _activesReviewModel.ChartMinWidth;
    }
    
    public SolidColorPaint TooltipTextPaint
    {
        get => _chartTooltipModel.TooltipTextPaint;
    }

    public SolidColorPaint TooltipBackgroundPaint
    {
        get => _chartTooltipModel.TooltipBackgroundPaint;
    }

    public bool? IsTitleOrdering
    {
        get => _activesReviewModel.IsTitleOrdering;
        set => _activesReviewModel.IsTitleOrdering = value;
    }

    public bool? IsCountOrdering
    {
        get => _activesReviewModel.IsCountOrdering;
        set => _activesReviewModel.IsCountOrdering = value;
    }

    public bool? IsBuySumOrdering
    {
        get => _activesReviewModel.IsBuySumOrdering;
        set => _activesReviewModel.IsBuySumOrdering = value;
    }

    public bool? IsCurrentSumOrdering
    {
        get => _activesReviewModel.IsCurrentSumOrdering;
        set => _activesReviewModel.IsCurrentSumOrdering = value;
    }

    public bool? IsChangeOrdering
    {
        get => _activesReviewModel.IsChangeOrdering;
        set => _activesReviewModel.IsChangeOrdering = value;
    }

    public IEnumerable<ActiveGroupViewModel> ActiveGroupModels
    {
        get => _activesReviewModel.ActiveGroupModels;
    }

    public ActiveGroupViewModel? SelectedActiveGroupModel
    {
        get => _activesReviewModel.SelectedActiveGroupModel;
        set => _activesReviewModel.SelectedActiveGroupModel = value;
    }

    public string? NotFoundText
    {
        get => _activesReviewModel.NotFoundText;
    }

    public bool IsLoading
    {
        get => _activesReviewModel.IsLoading;
    }

    #endregion Properties

    #region Commands

    public RelayCommand AttachedToVisualTreeCommand
    {
        get => _activesReviewModel.AttachedToVisualTreeCommand;
    }
    
    public RelayCommand AddActiveGroupCommand
    {
        get => _activeGroupsModel.AddActiveGroupCommand;
    }

    #endregion Commands

    #region Constructor

    public ActivesReviewViewModel(
        ActivesReviewModel activesReviewModel,
        ActiveGroupsModel activeGroupsModel,
        ChartTooltipModel chartTooltipModel)
    {
        _activesReviewModel = activesReviewModel;
        _activeGroupsModel = activeGroupsModel;
        _chartTooltipModel = chartTooltipModel;
        
        activesReviewModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        activeGroupsModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        chartTooltipModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
    
    #region Methods

    public void UpdateGroups()
    {
        _activesReviewModel.UpdateGroups();
    }
    
    #endregion Methods
}
