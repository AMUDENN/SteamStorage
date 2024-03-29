﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Services.ThemeService;
using SteamStorage.ViewModels.UtilityViewModels;
using SteamStorageAPI;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Utilities;

namespace SteamStorage.Models;

public class InventoryModel : ModelBase
{
    #region Constants

    private const string EMPTY_LIST_TEXT = "Элементы не найдены";

    #endregion Constants

    #region Fields

    private readonly ApiClient _apiClient;
    private readonly ChartTooltipModel _chartTooltipModel;
    private readonly UserModel _userModel;
    private readonly IThemeService _themeService;

    private GameModel? _selectedGameModel;
    private bool _isAllGamesChecked;

    private string? _filter;

    private bool? _isTitleOrdering;
    private bool? _isCountOrdering;
    private bool? _isPriceOrdering;
    private bool? _isSumOrdering;

    private bool _isRefreshing;

    private Inventory.InventoryOrderName? _inventoryOrderName;
    private bool? _isAscending;

    private List<InventoryItemViewModel> _inventoryModels;
    private InventoryItemViewModel? _selectedInventoryModel;

    private bool _isLoading;
    private CancellationTokenSource _cancellationTokenSource;

    private readonly int _pageSize;
    private int? _pageNumber;
    private int _currentPageNumber;
    private int _pagesCount;

    private int _displayItemsCountStart;
    private int _displayItemsCountEnd;
    private int _savedItemsCount;

    #endregion Fields

    #region Properties

    public GameModel? SelectedGameModel
    {
        get => _selectedGameModel;
        set
        {
            SetProperty(ref _selectedGameModel, value);
            if (value is null) return;
            IsAllGamesChecked = false;
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
            GetSkinsAsync();
        }
    }

    public string? Filter
    {
        get => _filter;
        set
        {
            SetProperty(ref _filter, value);
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
                InventoryOrderName = Inventory.InventoryOrderName.Title;
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
                InventoryOrderName = Inventory.InventoryOrderName.Count;
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
                InventoryOrderName = Inventory.InventoryOrderName.Price;
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
                InventoryOrderName = Inventory.InventoryOrderName.Sum;
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

    public string? NotFoundText
    {
        get => InventoryModels.Count == 0 && !IsLoading ? EMPTY_LIST_TEXT : null;
    }

    public bool IsLoading
    {
        get => _isLoading;
        private set
        {
            SetProperty(ref _isLoading, value);
            OnPropertyChanged(nameof(NotFoundText));
        }
    }

    private int PageSize
    {
        get => _pageSize;
        init
        {
            SetProperty(ref _pageSize, value);
            GetSkinsAsync();
        }
    }

    public int? PageNumber
    {
        get => _pageNumber;
        set
        {
            SetProperty(ref _pageNumber, value);
            if (PageNumber is not null) GetSkinsAsync();
        }
    }

    public int CurrentPageNumber
    {
        get => _currentPageNumber;
        private set => SetProperty(ref _currentPageNumber, value);
    }

    public int PagesCount
    {
        get => _pagesCount;
        private set
        {
            SetProperty(ref _pagesCount, value);
            if (value < PageNumber)
            {
                PageNumber = value;
            }
        }
    }

    public int DisplayItemsCountStart
    {
        get => _displayItemsCountStart;
        private set => SetProperty(ref _displayItemsCountStart, value < 1 ? 1 : value);
    }

    public int DisplayItemsCountEnd
    {
        get => _displayItemsCountEnd;
        private set => SetProperty(ref _displayItemsCountEnd, value < PageSize ? PageSize : value);
    }

    public int SavedItemsCount
    {
        get => _savedItemsCount;
        private set => SetProperty(ref _savedItemsCount, value);
    }

    private Inventory.InventoryOrderName? InventoryOrderName
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

    private CancellationTokenSource CancellationTokenSource
    {
        get => _cancellationTokenSource;
        set => SetProperty(ref _cancellationTokenSource, value);
    }

    #endregion Properties

    #region Commands

    public RelayCommand ClearFiltersCommand { get; }

    public AsyncRelayCommand RefreshInventoryCommand { get; }

    public RelayCommand AttachedToVisualTreeCommand { get; }

    #endregion Commands

    #region Constructor

    public InventoryModel(
        ApiClient apiClient,
        ChartTooltipModel chartTooltipModel,
        UserModel userModel,
        IThemeService themeService)
    {
        _apiClient = apiClient;
        _chartTooltipModel = chartTooltipModel;
        _userModel = userModel;
        _themeService = themeService;

        userModel.UserChanged += UserChangedHandler;
        userModel.CurrencyChanged += CurrencyChangedHandler;

        _inventoryModels = [];
        _cancellationTokenSource = new();

        IsAllGamesChecked = true;

        SetOrderingsNull();

        IsLoading = false;

        PageNumber = 1;
        PageSize = 20;

        ClearFiltersCommand = new(DoClearFiltersCommand);
        RefreshInventoryCommand = new(DoRefreshInventoryCommand);
        AttachedToVisualTreeCommand = new(DoAttachedToVisualTreeCommand);
    }

    #endregion Constructor

    #region Methods

    private void UserChangedHandler(object? sender)
    {
        GetSkinsAsync();
    }

    private void CurrencyChangedHandler(object? sender)
    {
        GetSkinsAsync();
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

    private async Task DoRefreshInventoryCommand()
    {
        if (SelectedGameModel is null) return;

        IsRefreshing = true;

        await _apiClient.PostAsync(
            ApiConstants.ApiMethods.RefreshInventory,
            new Inventory.RefreshInventoryRequest(SelectedGameModel.Id));

        IsRefreshing = false;
        
        GetSkinsAsync();
    }

    private void DoAttachedToVisualTreeCommand()
    {
        GetSkinsAsync();
    }

    private void SetOrderingsNull()
    {
        IsTitleOrdering = null;
        IsCountOrdering = null;
        IsPriceOrdering = null;
        IsSumOrdering = null;
    }

    private async void GetSkinsAsync()
    {
        InventoryModels = [];
        if (_userModel.User is null) return;

        IsLoading = true;

        await CancellationTokenSource.CancelAsync();

        CancellationTokenSource = new();
        CancellationToken token = CancellationTokenSource.Token;

        CurrentPageNumber = PageNumber ?? 1;

        DisplayItemsCountStart = (CurrentPageNumber - 1) * PageSize + 1;
        DisplayItemsCountEnd = CurrentPageNumber * PageSize;

        Inventory.InventoriesResponse? inventoriesResponse =
            await _apiClient.GetAsync<Inventory.InventoriesResponse, Inventory.GetInventoryRequest>(
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
