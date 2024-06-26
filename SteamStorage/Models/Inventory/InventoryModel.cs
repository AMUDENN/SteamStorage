﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using SteamStorage.Models.Tools.BaseModels;
using SteamStorage.Models.Tools.UtilityModels;
using SteamStorage.Services.ThemeService;
using SteamStorage.Utilities.Events.Settings;
using SteamStorage.ViewModels.Tools.UtilityViewModels;
using SteamStorageAPI.SDK;
using SteamStorageAPI.SDK.Utilities;

namespace SteamStorage.Models.Inventory;

public class InventoryModel : BaseListModel
{
    #region Constants

    private const double CHART_WIDTH_DEFAULT = 200;

    #endregion Constants

    #region Fields

    private readonly ApiClient _apiClient;
    private readonly ChartTooltipModel _chartTooltipModel;
    private readonly UserModel _userModel;
    private readonly PeriodsModel _periodsModel;
    private readonly IThemeService _themeService;

    private int _count;
    private string _currentSumString;
    private IEnumerable<SteamStorageAPI.SDK.ApiEntities.Inventory.InventoryGameCountResponse> _inventoryGameCount;
    private ObservableCollection<ISeries> _inventoryGameCountSeries;
    private IEnumerable<SteamStorageAPI.SDK.ApiEntities.Inventory.InventoryGameSumResponse> _inventoryGameSum;
    private ObservableCollection<ISeries> _inventoryGameSumSeries;
    private double _chartWidth;

    private GameModel? _selectedGameModel;
    private bool _isAllGamesChecked;

    private string? _filter;

    private bool? _isTitleOrdering;
    private bool? _isCountOrdering;
    private bool? _isPriceOrdering;
    private bool? _isSumOrdering;

    private bool _isRefreshing;

    private SteamStorageAPI.SDK.ApiEntities.Inventory.InventoryOrderName? _inventoryOrderName;
    private bool? _isAscending;

    private List<InventoryItemViewModel> _inventoryModels;
    private InventoryItemViewModel? _selectedInventoryModel;

    private CancellationTokenSource _itemsCancellationTokenSource;
    private CancellationTokenSource _statisticsCancellationTokenSource;

    #endregion Fields

    #region Properties

    public int Count
    {
        get => _count;
        private set => SetProperty(ref _count, value);
    }

    public string CurrentSumString
    {
        get => _currentSumString;
        private set => SetProperty(ref _currentSumString, value);
    }

    private IEnumerable<SteamStorageAPI.SDK.ApiEntities.Inventory.InventoryGameCountResponse> InventoryGameCount
    {
        get => _inventoryGameCount;
        set
        {
            SetProperty(ref _inventoryGameCount, value);
            GetInventoryGameCountSeries();
        }
    }

    public ObservableCollection<ISeries> InventoryGameCountSeries
    {
        get => _inventoryGameCountSeries;
        private set => SetProperty(ref _inventoryGameCountSeries, value);
    }

    private IEnumerable<SteamStorageAPI.SDK.ApiEntities.Inventory.InventoryGameSumResponse> InventoryGameSum
    {
        get => _inventoryGameSum;
        set
        {
            SetProperty(ref _inventoryGameSum, value);
            GetInventoryGameSumSeries();
        }
    }

    public ObservableCollection<ISeries> InventoryGameSumSeries
    {
        get => _inventoryGameSumSeries;
        private set => SetProperty(ref _inventoryGameSumSeries, value);
    }

    public double ChartWidth
    {
        get => _chartWidth;
        private set => SetProperty(ref _chartWidth, value);
    }

    public GameModel? SelectedGameModel
    {
        get => _selectedGameModel;
        set
        {
            SetProperty(ref _selectedGameModel, value);
            if (value is null) return;
            IsAllGamesChecked = false;
            GetStatisticsAsync();
            GetSkinsAsync();
        }
    }

    public bool IsAllGamesChecked
    {
        get => _isAllGamesChecked;
        set
        {
            SetProperty(ref _isAllGamesChecked, value);
            if (!value) return;
            SelectedGameModel = null;
            GetStatisticsAsync();
            GetSkinsAsync();
        }
    }

    public string? Filter
    {
        get => _filter;
        set
        {
            SetProperty(ref _filter, value);
            GetStatisticsAsync();
            GetSkinsAsync();
        }
    }

    public bool? IsTitleOrdering
    {
        get => _isTitleOrdering;
        set
        {
            if (_isTitleOrdering is not null && value is null)
            {
                InventoryOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                InventoryOrderName = SteamStorageAPI.SDK.ApiEntities.Inventory.InventoryOrderName.Title;
                IsAscending = value;
            }

            SetProperty(ref _isTitleOrdering, value);
        }
    }

    public bool? IsCountOrdering
    {
        get => _isCountOrdering;
        set
        {
            if (_isCountOrdering is not null && value is null)
            {
                InventoryOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                InventoryOrderName = SteamStorageAPI.SDK.ApiEntities.Inventory.InventoryOrderName.Count;
                IsAscending = value;
            }

            SetProperty(ref _isCountOrdering, value);
        }
    }

    public bool? IsPriceOrdering
    {
        get => _isPriceOrdering;
        set
        {
            if (_isPriceOrdering is not null && value is null)
            {
                InventoryOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                InventoryOrderName = SteamStorageAPI.SDK.ApiEntities.Inventory.InventoryOrderName.Price;
                IsAscending = value;
            }

            SetProperty(ref _isPriceOrdering, value);
        }
    }

    public bool? IsSumOrdering
    {
        get => _isSumOrdering;
        set
        {
            if (_isSumOrdering is not null && value is null)
            {
                InventoryOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                InventoryOrderName = SteamStorageAPI.SDK.ApiEntities.Inventory.InventoryOrderName.Sum;
                IsAscending = value;
            }

            SetProperty(ref _isSumOrdering, value);
        }
    }

    public bool IsRefreshing
    {
        get => _isRefreshing;
        private set => SetProperty(ref _isRefreshing, value);
    }

    public List<InventoryItemViewModel> InventoryModels
    {
        get => _inventoryModels;
        private set
        {
            SetProperty(ref _inventoryModels, value);
            OnPropertyChanged(nameof(NotFoundText));
        }
    }

    public InventoryItemViewModel? SelectedInventoryModel
    {
        get => _selectedInventoryModel;
        set
        {
            SetProperty(ref _selectedInventoryModel, value);
            SelectedInventoryModel?.UpdateStats();
        }
    }

    public override string? NotFoundText
    {
        get => InventoryModels.Count == 0 && !IsLoading ? EMPTY_LIST_TEXT : null;
    }

    private SteamStorageAPI.SDK.ApiEntities.Inventory.InventoryOrderName? InventoryOrderName
    {
        get => _inventoryOrderName;
        set
        {
            SetProperty(ref _inventoryOrderName, value);
            GetSkinsAsync();
        }
    }

    private bool? IsAscending
    {
        get => _isAscending;
        set
        {
            SetProperty(ref _isAscending, value);
            GetSkinsAsync();
        }
    }

    private CancellationTokenSource ItemsCancellationTokenSource
    {
        get => _itemsCancellationTokenSource;
        set => SetProperty(ref _itemsCancellationTokenSource, value);
    }

    private CancellationTokenSource StatisticsCancellationTokenSource
    {
        get => _statisticsCancellationTokenSource;
        set => SetProperty(ref _statisticsCancellationTokenSource, value);
    }

    #endregion Properties

    #region Commands

    public RelayCommand ClearFiltersCommand { get; }

    public AsyncRelayCommand RefreshInventoryCommand { get; }

    #endregion Commands

    #region Constructor

    public InventoryModel(
        ApiClient apiClient,
        ChartTooltipModel chartTooltipModel,
        UserModel userModel,
        PeriodsModel periodsModel,
        IThemeService themeService)
    {
        _apiClient = apiClient;
        _chartTooltipModel = chartTooltipModel;
        _userModel = userModel;
        _periodsModel = periodsModel;
        _themeService = themeService;

        userModel.UserChanged += UserChangedHandler;
        userModel.CurrencyChanged += CurrencyChangedHandler;

        themeService.ChartThemeChanged += ChartThemeChangedHandler;

        _currentSumString = string.Empty;

        _inventoryGameCount = Enumerable.Empty<SteamStorageAPI.SDK.ApiEntities.Inventory.InventoryGameCountResponse>();
        _inventoryGameCountSeries = [];
        _inventoryGameSum = Enumerable.Empty<SteamStorageAPI.SDK.ApiEntities.Inventory.InventoryGameSumResponse>();
        _inventoryGameSumSeries = [];
        _chartWidth = CHART_WIDTH_DEFAULT;

        _inventoryModels = [];
        _itemsCancellationTokenSource = new();
        _statisticsCancellationTokenSource = new();

        IsAllGamesChecked = true;

        SetOrderingsNull();

        IsLoading = false;

        PageNumber = 1;
        PageSize = 20;

        ClearFiltersCommand = new(DoClearFiltersCommand);
        RefreshInventoryCommand = new(DoRefreshInventoryCommand);
    }

    #endregion Constructor

    #region Methods

    private void UserChangedHandler(object? sender)
    {
        GetSkinsAsync();
        GetStatisticsAsync();
    }

    private void CurrencyChangedHandler(object? sender)
    {
        GetSkinsAsync();
        GetStatisticsAsync();
    }

    private void ChartThemeChangedHandler(object? sender, ChartThemeChangedEventArgs args)
    {
        GetInventoryGameCountSeries();
        GetInventoryGameSumSeries();
    }

    private void DoClearFiltersCommand()
    {
        Filter = null;
        IsAllGamesChecked = true;
        InventoryOrderName = null;
        IsAscending = null;
        PageNumber = 1;
        SetOrderingsNull();
    }

    private async Task DoRefreshInventoryCommand(CancellationToken cancellationToken)
    {
        if (SelectedGameModel is null) return;

        IsRefreshing = true;

        await _apiClient.PostAsync(
            ApiConstants.ApiMethods.RefreshInventory,
            new SteamStorageAPI.SDK.ApiEntities.Inventory.RefreshInventoryRequest(SelectedGameModel.Id),
            cancellationToken);

        IsRefreshing = false;

        GetSkinsAsync();
        GetStatisticsAsync();
    }

    private void SetOrderingsNull()
    {
        IsTitleOrdering = null;
        IsCountOrdering = null;
        IsPriceOrdering = null;
        IsSumOrdering = null;
    }

    private void GetInventoryGameCountSeries()
    {
        if (!InventoryGameCount.Any())
        {
            InventoryGameCountSeries = [];
        }
        else
        {
            int i = 0;

            InventoryGameCountSeries = new(InventoryGameCount.OrderByDescending(x => x.GameTitle)
                .AsPieSeries((value, builder) =>
                {
                    builder.MaxRadialColumnWidth = 20;
                    builder.HoverPushout = 0;
                    builder.Mapping = (game, point) => new(point, game.Count);
                    builder.ToolTipLabelFormatter = _ => $"{value.GameTitle}: {value.Count:N0}";
                    builder.Fill =
                        new SolidColorPaint(_themeService.CurrentChartThemeVariant.Colors.ElementAt(i).Color);
                    i++;
                }));
        }

        ChartWidth = ChartWidth < CHART_WIDTH_DEFAULT ? ChartWidth + 1 : ChartWidth - 1;
    }

    private void GetInventoryGameSumSeries()
    {
        if (!InventoryGameSum.Any())
        {
            InventoryGameSumSeries = [];
        }
        else
        {
            int i = 0;

            InventoryGameSumSeries = new(InventoryGameSum.OrderByDescending(x => x.GameTitle)
                .AsPieSeries((value, builder) =>
                {
                    builder.MaxRadialColumnWidth = 20;
                    builder.HoverPushout = 0;
                    builder.Mapping = (game, point) => new(point, (double)game.Sum);
                    builder.ToolTipLabelFormatter = _ => $"{value.GameTitle}: {value.Sum:N2}";
                    builder.Fill =
                        new SolidColorPaint(_themeService.CurrentChartThemeVariant.Colors.ElementAt(i).Color);
                    i++;
                }));
        }

        ChartWidth = ChartWidth < CHART_WIDTH_DEFAULT ? ChartWidth + 1 : ChartWidth - 1;
    }

    private async void GetStatisticsAsync()
    {
        await StatisticsCancellationTokenSource.CancelAsync();

        StatisticsCancellationTokenSource = new();
        CancellationToken token = StatisticsCancellationTokenSource.Token;

        SteamStorageAPI.SDK.ApiEntities.Inventory.InventoriesStatisticResponse? inventoriesStatisticResponse =
            await _apiClient
                .GetAsync<SteamStorageAPI.SDK.ApiEntities.Inventory.InventoriesStatisticResponse,
                    SteamStorageAPI.SDK.ApiEntities.Inventory.GetInventoriesStatisticRequest>(
                    ApiConstants.ApiMethods.GetInventoriesStatistic,
                    new(SelectedGameModel?.Id, Filter),
                    token);

        if (inventoriesStatisticResponse is null) return;

        Count = inventoriesStatisticResponse.InventoriesCount;
        CurrentSumString = $"{inventoriesStatisticResponse.CurrentSum:N2} {_userModel.CurrencyMark}";

        if (inventoriesStatisticResponse.GameCount is not null)
            InventoryGameCount = inventoriesStatisticResponse.GameCount;

        if (inventoriesStatisticResponse.GameSum is not null)
            InventoryGameSum = inventoriesStatisticResponse.GameSum;
    }

    protected override async void GetSkinsAsync()
    {
        InventoryModels = [];
        if (_userModel.User is null) return;

        IsLoading = true;

        await ItemsCancellationTokenSource.CancelAsync();

        ItemsCancellationTokenSource = new();
        CancellationToken token = ItemsCancellationTokenSource.Token;

        CurrentPageNumber = PageNumber ?? 1;

        DisplayItemsCountStart = (CurrentPageNumber - 1) * PageSize + 1;
        DisplayItemsCountEnd = CurrentPageNumber * PageSize;

        SteamStorageAPI.SDK.ApiEntities.Inventory.InventoriesResponse? inventoriesResponse =
            await _apiClient
                .GetAsync<SteamStorageAPI.SDK.ApiEntities.Inventory.InventoriesResponse,
                    SteamStorageAPI.SDK.ApiEntities.Inventory.GetInventoryRequest>(
                    ApiConstants.ApiMethods.GetInventory,
                    new(SelectedGameModel?.Id, Filter, InventoryOrderName, IsAscending, CurrentPageNumber, PageSize),
                    token);

        if (inventoriesResponse is null) return;

        SavedItemsCount = inventoriesResponse.Count;
        PagesCount = inventoriesResponse.PagesCount;

        if (inventoriesResponse.Inventories is null) return;

        InventoryModels = inventoriesResponse.Inventories.Select(x =>
                new InventoryItemViewModel(
                    new(_apiClient,
                        _periodsModel,
                        _themeService,
                        x.Skin.Id,
                        x.Skin.SkinIconUrl,
                        x.Skin.MarketUrl,
                        x.Skin.Title,
                        x.Count,
                        x.CurrentPrice,
                        x.CurrentSum,
                        _userModel.CurrencyMark),
                    _chartTooltipModel))
            .ToList();

        IsLoading = false;
    }

    #endregion Methods
}
