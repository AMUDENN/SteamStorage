using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Painting;
using SteamStorage.Models;
using SteamStorage.Models.Inventory;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.ViewModels.Tools;
using SteamStorage.ViewModels.Tools.UtilityViewModels;

namespace SteamStorage.ViewModels.Inventory;

public class InventoryViewModel : ViewModelBase
{
    #region Fields

    private readonly InventoryModel _inventoryModel;
    private readonly GamesModel _gamesModel;
    private readonly ChartTooltipModel _chartTooltipModel;

    #endregion Fields

    #region Properties

    public IEnumerable<GameModel> GameModels => _gamesModel.GameModels;

    public int Count => _inventoryModel.Count;

    public string CurrentSumString => _inventoryModel.CurrentSumString;

    public ObservableCollection<ISeries> InventoryGameCountSeries => _inventoryModel.InventoryGameCountSeries;

    public ObservableCollection<ISeries> InventoryGameSumSeries => _inventoryModel.InventoryGameSumSeries;

    public double ChartWidth => _inventoryModel.ChartWidth;

    public SolidColorPaint TooltipTextPaint => _chartTooltipModel.TooltipTextPaint;

    public SolidColorPaint TooltipBackgroundPaint => _chartTooltipModel.TooltipBackgroundPaint;

    public GameModel? SelectedGameModel
    {
        get => _inventoryModel.SelectedGameModel;
        set => _inventoryModel.SelectedGameModel = value;
    }

    public bool IsAllGamesChecked
    {
        get => _inventoryModel.IsAllGamesChecked;
        set => _inventoryModel.IsAllGamesChecked = value;
    }

    public string? Filter
    {
        get => _inventoryModel.Filter;
        set => _inventoryModel.Filter = value;
    }

    public bool? IsTitleOrdering
    {
        get => _inventoryModel.IsTitleOrdering;
        set => _inventoryModel.IsTitleOrdering = value;
    }

    public bool? IsCountOrdering
    {
        get => _inventoryModel.IsCountOrdering;
        set => _inventoryModel.IsCountOrdering = value;
    }

    public bool? IsPriceOrdering
    {
        get => _inventoryModel.IsPriceOrdering;
        set => _inventoryModel.IsPriceOrdering = value;
    }

    public bool? IsSumOrdering
    {
        get => _inventoryModel.IsSumOrdering;
        set => _inventoryModel.IsSumOrdering = value;
    }

    public bool IsRefreshing => _inventoryModel.IsRefreshing;

    public IEnumerable<InventoryItemViewModel> InventoryModels => _inventoryModel.InventoryModels;

    public InventoryItemViewModel? SelectedInventoryModel
    {
        get => _inventoryModel.SelectedInventoryModel;
        set => _inventoryModel.SelectedInventoryModel = value;
    }

    public string? NotFoundText => _inventoryModel.NotFoundText;

    public bool IsLoading => _inventoryModel.IsLoading;

    public int? PageNumber
    {
        get => _inventoryModel.PageNumber;
        set => _inventoryModel.PageNumber = value;
    }

    public int CurrentPageNumber => _inventoryModel.CurrentPageNumber;

    public int PagesCount => _inventoryModel.PagesCount;

    public int DisplayItemsCountStart => _inventoryModel.DisplayItemsCountStart;

    public int DisplayItemsCountEnd => _inventoryModel.DisplayItemsCountEnd;

    public int SavedItemsCount => _inventoryModel.SavedItemsCount;

    #endregion Properties

    #region Commands

    public RelayCommand ClearFiltersCommand => _inventoryModel.ClearFiltersCommand;

    public AsyncRelayCommand RefreshInventoryCommand => _inventoryModel.RefreshInventoryCommand;

    #endregion Commands

    #region Constructor

    public InventoryViewModel(
        InventoryModel inventoryModel,
        GamesModel gamesModel,
        ChartTooltipModel chartTooltipModel)
    {
        _inventoryModel = inventoryModel;
        _gamesModel = gamesModel;
        _chartTooltipModel = chartTooltipModel;

        inventoryModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        gamesModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        chartTooltipModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
    }

    #endregion Constructor
}