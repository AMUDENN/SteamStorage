using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
using SteamStorageAPI;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Utilities;

namespace SteamStorage.Models;

public class ListItemsModel : ModelBase
{
    #region Constants

    private const string EMPTY_LIST_TEXT = "Элементы не найдены";

    #endregion Constants

    #region Fields

    private readonly ApiClient _apiClient;
    private readonly UserModel _userModel;

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

    private List<ListItemModel> _listItemModels;
    private ListItemModel? _selectedListItemModel;

    private bool _isLoading;
    private CancellationTokenSource _cancellationTokenSource;

    private int _pageSize;
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

    public List<ListItemModel> ListItemModels
    {
        get => _listItemModels;
        private set
        {
            SetProperty(ref _listItemModels, value);
            OnPropertyChanged(nameof(NotFoundText));
        }
    }

    public ListItemModel? SelectedListItemModel
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

    public int PageSize
    {
        get => _pageSize;
        private set
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

    #endregion Commands

    #region Constructor

    public ListItemsModel(ApiClient apiClient, UserModel userModel)
    {
        _apiClient = apiClient;
        _userModel = userModel;

        _listItemModels = [];
        _cancellationTokenSource = new();

        userModel.UserChanged += UserChangedHandler;
        userModel.CurrencyChanged += CurrencyChangedHandler;

        IsAllGamesChecked = true;

        SetOrderingsNull();

        IsMarked = false;

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
        SkinOrderName = null;
        IsAscending = null;
        IsMarked = false;
        PageNumber = 1;
        SetOrderingsNull();
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
                ApiConstants.ApiControllers.Skins,
                "GetSkins",
                new(SelectedGameModel?.Id, Filter, SkinOrderName, IsAscending, IsMarked ? IsMarked : null,
                    CurrentPageNumber, PageSize),
                token);

        if (skinsResponse is null) return;

        SavedItemsCount = skinsResponse.SkinsCount;
        PagesCount = skinsResponse.PagesCount;

        ListItemModels = skinsResponse.Skins.Select(x =>
                new ListItemModel(_apiClient, x.Skin.Id, x.Skin.SkinIconUrl, x.Skin.Title, x.CurrentPrice,
                    _userModel.CurrencyMark, x.Change7D, x.Change30D, x.IsMarked))
            .ToList();

        IsLoading = false;
    }

    #endregion Methods
}
