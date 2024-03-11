using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Services.ThemeService;
using SteamStorage.ViewModels.UtilityViewModels;
using SteamStorageAPI;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Utilities;

namespace SteamStorage.Models;

public class ListActivesModel : ModelBase
{
    #region Constants

    private const string EMPTY_LIST_TEXT = "Элементы не найдены";

    #endregion Constants

    #region Fields

    private readonly ApiClient _apiClient;
    private readonly ChartTooltipModel _chartTooltipModel;
    private readonly UserModel _userModel;
    private readonly IThemeService _themeService;

    private BaseGroupModel? _selectedGroupModel;

    private GameModel? _selectedGameModel;
    private bool _isAllGamesChecked;

    private string? _filter;

    private bool? _isTitleOrdering;
    private bool? _isCountOrdering;
    private bool? _isBuyPriceOrdering;
    private bool? _isCurrentPriceOrdering;
    private bool? _isCurrentSumOrdering;
    private bool? _isChangeOrdering;

    private Actives.ActiveOrderName? _activeOrderName;
    private bool? _isAscending;

    private List<ActiveViewModel> _activeModels;
    private ActiveViewModel? _selectedActiveModel;

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

    public BaseGroupModel? SelectedGroupModel
    {
        get => _selectedGroupModel;
        set
        {
            SetProperty(ref _selectedGroupModel, value);
            GetSkins();
        }
    }

    public GameModel? SelectedGameModel
    {
        get => _selectedGameModel;
        set
        {
            SetProperty(ref _selectedGameModel, value);
            if (value is null) return;
            IsAllGamesChecked = false;
            GetSkins();
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
            GetSkins();
        }
    }

    public string? Filter
    {
        get => _filter;
        set
        {
            SetProperty(ref _filter, value);
            GetSkins();
        }
    }

    public bool? IsTitleOrdering
    {
        get => _isTitleOrdering;
        set
        {
            if (_isTitleOrdering is not null && value is null)
            {
                ActiveOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ActiveOrderName = Actives.ActiveOrderName.Title;
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
                ActiveOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ActiveOrderName = Actives.ActiveOrderName.Count;
                IsAscending = value;
            }

            SetProperty(ref _isCountOrdering, value);
        }
    }

    public bool? IsBuyPriceOrdering
    {
        get => _isBuyPriceOrdering;
        set
        {
            if (_isBuyPriceOrdering is not null && value is null)
            {
                ActiveOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ActiveOrderName = Actives.ActiveOrderName.BuyPrice;
                IsAscending = value;
            }

            SetProperty(ref _isBuyPriceOrdering, value);
        }
    }

    public bool? IsCurrentPriceOrdering
    {
        get => _isCurrentPriceOrdering;
        set
        {
            if (_isCurrentPriceOrdering is not null && value is null)
            {
                ActiveOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ActiveOrderName = Actives.ActiveOrderName.CurrentPrice;
                IsAscending = value;
            }

            SetProperty(ref _isCurrentPriceOrdering, value);
        }
    }

    public bool? IsCurrentSumOrdering
    {
        get => _isCurrentSumOrdering;
        set
        {
            if (_isCurrentSumOrdering is not null && value is null)
            {
                ActiveOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ActiveOrderName = Actives.ActiveOrderName.CurrentSum;
                IsAscending = value;
            }

            SetProperty(ref _isCurrentSumOrdering, value);
        }
    }

    public bool? IsChangeOrdering
    {
        get => _isChangeOrdering;
        set
        {
            if (_isChangeOrdering is not null && value is null)
            {
                ActiveOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ActiveOrderName = Actives.ActiveOrderName.Change;
                IsAscending = value;
            }

            SetProperty(ref _isChangeOrdering, value);
        }
    }

    public List<ActiveViewModel> ActiveModels
    {
        get => _activeModels;
        private set
        {
            SetProperty(ref _activeModels, value);
            OnPropertyChanged(nameof(NotFoundText));
        }
    }

    public ActiveViewModel? SelectedActiveModel
    {
        get => _selectedActiveModel;
        set
        {
            SetProperty(ref _selectedActiveModel, value);
            SelectedActiveModel?.UpdateStats();
        }
    }

    public string? NotFoundText
    {
        get => ActiveModels.Count == 0 && !IsLoading ? EMPTY_LIST_TEXT : null;
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
            GetSkins();
        }
    }

    public int? PageNumber
    {
        get => _pageNumber;
        set
        {
            SetProperty(ref _pageNumber, value);
            if (PageNumber is not null) GetSkins();
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

    private Actives.ActiveOrderName? ActiveOrderName
    {
        get => _activeOrderName;
        set
        {
            SetProperty(ref _activeOrderName, value);
            GetSkins();
        }
    }

    private bool? IsAscending
    {
        get => _isAscending;
        set
        {
            SetProperty(ref _isAscending, value);
            GetSkins();
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

    #endregion Commands

    #region Constructor

    public ListActivesModel(
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

        _activeModels = [];
        _cancellationTokenSource = new();

        IsAllGamesChecked = true;

        SetOrderingsNull();

        IsLoading = false;

        PageNumber = 1;
        PageSize = 20;
        
        ClearFiltersCommand = new(DoClearFiltersCommand);
    }

    #endregion Constructor

    #region Methods

    private void UserChangedHandler(object? sender)
    {
        GetSkins();
    }

    private void CurrencyChangedHandler(object? sender)
    {
        GetSkins();
    }

    private void DoClearFiltersCommand()
    {
        Filter = null;
        IsAllGamesChecked = true;
        ActiveOrderName = null;
        IsAscending = null;
        PageNumber = 1;
        SetOrderingsNull();
    }

    private void SetOrderingsNull()
    {
        IsTitleOrdering = null;
        IsCountOrdering = null;
        IsBuyPriceOrdering = null;
        IsCurrentPriceOrdering = null;
        IsCurrentSumOrdering = null;
        IsChangeOrdering = null;
    }

    private async void GetSkins()
    {
        ActiveModels = [];
        if (_userModel.User is null) return;

        IsLoading = true;

        await CancellationTokenSource.CancelAsync();

        CancellationTokenSource = new();
        CancellationToken token = CancellationTokenSource.Token;

        CurrentPageNumber = PageNumber ?? 1;

        DisplayItemsCountStart = (CurrentPageNumber - 1) * PageSize + 1;
        DisplayItemsCountEnd = CurrentPageNumber * PageSize;

        Actives.ActivesResponse? activesResponse =
            await _apiClient.GetAsync<Actives.ActivesResponse, Actives.GetActivesRequest>(
                ApiConstants.ApiControllers.Actives,
                "GetActives",
                new(SelectedGroupModel?.GroupId, SelectedGameModel?.Id, Filter, ActiveOrderName, IsAscending,
                    CurrentPageNumber, PageSize),
                token);

        if (activesResponse is null) return;

        SavedItemsCount = activesResponse.Count;
        PagesCount = activesResponse.PagesCount;

        ActiveModels = activesResponse.Actives.Select(x =>
                new ActiveViewModel(
                    new(_apiClient, _themeService, x.Skin.Id, x.Skin.SkinIconUrl, x.Skin.MarketUrl, x.Skin.Title,
                        x.Count, x.BuyPrice, x.CurrentPrice, x.CurrentSum, x.GoalPrice, x.GoalPriceCompletion,
                        _userModel.CurrencyMark, x.Change, x.BuyDate),
                    _chartTooltipModel))
            .ToList();

        IsLoading = false;
    }

    #endregion Methods
}
