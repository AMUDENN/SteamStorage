using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Services.ThemeService;
using SteamStorage.Utilities.Events.ListItems;
using SteamStorage.ViewModels.UtilityViewModels;
using SteamStorageAPI;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Utilities;

namespace SteamStorage.Models;

public class ListItemsModel : ModelBase
{
    #region Constants

    private const string EMPTY_LIST_TEXT = "Элементы не найдены";

    #endregion Constants

    #region Events

    public delegate void AddToActivesEventHandler(object? sender, AddToActivesEventArgs args);

    public event AddToActivesEventHandler? AddToActives;

    public delegate void AddToArchiveEventHandler(object? sender, AddToArchiveEventArgs args);

    public event AddToArchiveEventHandler? AddToArchive;

    #endregion Events

    #region Fields

    private readonly ApiClient _apiClient;
    private readonly ChartTooltipModel _chartTooltipModel;
    private readonly UserModel _userModel;
    private readonly IThemeService _themeService;

    private GameModel? _selectedGameModel;
    private bool _isAllGamesChecked;

    private string? _filter;

    private bool? _isTitleOrdering;
    private bool? _isCurrentCostOrdering;
    private bool? _isChange7Ordering;
    private bool? _isChange30Ordering;

    private Skins.SkinOrderName? _skinOrderName;
    private bool? _isAscending;

    private bool _isMarked;

    private List<ListItemViewModel> _listItemModels;
    private ListItemViewModel? _selectedListItemModel;

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
                SkinOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                SkinOrderName = Skins.SkinOrderName.Title;
                IsAscending = value;
            }

            SetProperty(ref _isTitleOrdering, value);
        }
    }

    public bool? IsCurrentCostOrdering
    {
        get => _isCurrentCostOrdering;
        set
        {
            if (_isCurrentCostOrdering is not null && value is null)
            {
                SkinOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                SkinOrderName = Skins.SkinOrderName.Price;
                IsAscending = value;
            }

            SetProperty(ref _isCurrentCostOrdering, value);
        }
    }

    public bool? IsChange7Ordering
    {
        get => _isChange7Ordering;
        set
        {
            if (_isChange7Ordering is not null && value is null)
            {
                SkinOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                SkinOrderName = Skins.SkinOrderName.Change7D;
                IsAscending = value;
            }

            SetProperty(ref _isChange7Ordering, value);
        }
    }

    public bool? IsChange30Ordering
    {
        get => _isChange30Ordering;
        set
        {
            if (_isChange30Ordering is not null && value is null)
            {
                SkinOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                SkinOrderName = Skins.SkinOrderName.Change30D;
                IsAscending = value;
            }

            SetProperty(ref _isChange30Ordering, value);
        }
    }

    public bool IsMarked
    {
        get => _isMarked;
        set
        {
            SetProperty(ref _isMarked, value);
            GetSkins();
        }
    }

    public List<ListItemViewModel> ListItemModels
    {
        get => _listItemModels;
        private set
        {
            SetProperty(ref _listItemModels, value);
            OnPropertyChanged(nameof(NotFoundText));
        }
    }

    public ListItemViewModel? SelectedListItemModel
    {
        get => _selectedListItemModel;
        set
        {
            SetProperty(ref _selectedListItemModel, value);
            SelectedListItemModel?.UpdateStats();
        }
    }

    public string? NotFoundText
    {
        get => ListItemModels.Count == 0 && !IsLoading ? EMPTY_LIST_TEXT : null;
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

    private Skins.SkinOrderName? SkinOrderName
    {
        get => _skinOrderName;
        set
        {
            SetProperty(ref _skinOrderName, value);
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

    public RelayCommand<ListItemModel> AddToActivesCommand { get; }

    public RelayCommand<ListItemModel> AddToArchiveCommand { get; }

    #endregion Commands

    #region Constructor

    public ListItemsModel(
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

        _listItemModels = [];
        _cancellationTokenSource = new();

        IsAllGamesChecked = true;

        SetOrderingsNull();

        IsMarked = false;

        IsLoading = false;

        PageNumber = 1;
        PageSize = 20;

        ClearFiltersCommand = new(DoClearFiltersCommand);
        AddToActivesCommand = new(DoAddToActivesCommand);
        AddToArchiveCommand = new(DoAddToArchiveCommand);
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
        SkinOrderName = null;
        IsAscending = null;
        IsMarked = false;
        PageNumber = 1;
        SetOrderingsNull();
    }

    private void DoAddToActivesCommand(ListItemModel? model)
    {
        OnAddToActives(model);
    }

    private void DoAddToArchiveCommand(ListItemModel? model)
    {
        OnAddToArchive(model);
    }

    private void SetOrderingsNull()
    {
        IsTitleOrdering = null;
        IsCurrentCostOrdering = null;
        IsChange7Ordering = null;
        IsChange30Ordering = null;
    }

    private async void GetSkins()
    {
        ListItemModels = [];
        if (_userModel.User is null) return;

        IsLoading = true;

        await CancellationTokenSource.CancelAsync();

        CancellationTokenSource = new();
        CancellationToken token = CancellationTokenSource.Token;

        CurrentPageNumber = PageNumber ?? 1;

        DisplayItemsCountStart = (CurrentPageNumber - 1) * PageSize + 1;
        DisplayItemsCountEnd = CurrentPageNumber * PageSize;

        Skins.SkinsResponse? skinsResponse =
            await _apiClient.GetAsync<Skins.SkinsResponse, Skins.GetSkinsRequest>(
                ApiConstants.ApiMethods.GetSkins,
                new(SelectedGameModel?.Id, Filter, SkinOrderName, IsAscending, IsMarked ? IsMarked : null,
                    CurrentPageNumber, PageSize),
                token);

        if (skinsResponse is null) return;

        SavedItemsCount = skinsResponse.Count;
        PagesCount = skinsResponse.PagesCount;
        
        if (skinsResponse.Skins is null) return;

        ListItemModels = skinsResponse.Skins.Select(x =>
                new ListItemViewModel(
                    new(_apiClient,
                        _themeService,
                        x.Skin.Id,
                        x.Skin.SkinIconUrl,
                        x.Skin.MarketUrl,
                        x.Skin.Title,
                        x.CurrentPrice,
                        _userModel.CurrencyMark,
                        x.Change7D,
                        x.Change30D,
                        x.IsMarked),
                    this,
                    _chartTooltipModel))
            .ToList();

        IsLoading = false;
    }

    private void OnAddToActives(ListItemModel? model)
    {
        AddToActives?.Invoke(this, new(model));
    }

    private void OnAddToArchive(ListItemModel? model)
    {
        AddToArchive?.Invoke(this, new(model));
    }

    #endregion Methods
}
