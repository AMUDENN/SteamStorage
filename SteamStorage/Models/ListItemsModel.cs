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

    private string? _notFoundText;
    private bool _isLoading;
    private CancellationTokenSource _cancellationTokenSource;

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
            if (value.Count == 0 && !IsLoading) NotFoundText = EMPTY_LIST_TEXT;
        }
    }

    public string? NotFoundText
    {
        get => _notFoundText;
        private set => SetProperty(ref _notFoundText, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        private set
        {
            SetProperty(ref _isLoading, value);
            if (value) NotFoundText = null;
        }
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

        ClearFiltersCommand = new(DoClearFiltersCommand);
    }

    #endregion Constructor

    #region Methods

    private void DoClearFiltersCommand()
    {
        IsAllGamesChecked = true;
        Filter = null;
        SkinOrderName = null;
        IsAscending = null;
        IsMarked = false;
        SetOrderingsNull();
    }

    private void SetOrderingsNull()
    {
        IsTitleOrdering = null;
        IsCurrentCostOrdering = null;
        IsChange7Ordering = null;
        IsChange30Ordering = null;
    }

    private void UserChangedHandler(object sender)
    {
        GetSkins();
    }
    
    private void CurrencyChangedHandler(object sender)
    {
        GetSkins();
    }

    private async void GetSkins()
    {
        ListItemModels = [];
        if (_userModel.User is null) return;

        IsLoading = true;
        
        await CancellationTokenSource.CancelAsync();

        CancellationTokenSource = new();
        CancellationToken token = CancellationTokenSource.Token;
        

        List<Skins.SkinResponse>? skinsResponse =
            await _apiClient.GetAsync<List<Skins.SkinResponse>, Skins.GetSkinsRequest>(
                ApiConstants.ApiControllers.Skins,
                "GetSkins",
                new(SelectedGameModel?.Id, Filter, SkinOrderName, IsAscending, IsMarked ? IsMarked : null, 1, 30),
                token);
        if (skinsResponse is null) return;

        ListItemModels = skinsResponse.Select(x =>
                new ListItemModel(x.Skin.Id, x.Skin.SkinIconUrl, x.Skin.Title, x.CurrentPrice, _userModel.CurrencyMark, x.Change7D, x.Change30D,
                    x.IsMarked))
            .ToList();

        IsLoading = false;
    }

    #endregion Methods
}
