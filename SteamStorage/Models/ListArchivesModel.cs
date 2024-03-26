using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorage.Models.UtilityModels;
using SteamStorage.Models.UtilityModels.BaseModels;
using SteamStorage.Utilities.Events.Archives;
using SteamStorage.ViewModels.UtilityViewModels;
using SteamStorageAPI;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Utilities;

namespace SteamStorage.Models;

public class ListArchivesModel : ModelBase
{
    #region Constants

    private const string EMPTY_LIST_TEXT = "Элементы не найдены";

    #endregion Constants

    #region Events

    public delegate void EditArchiveEventHandler(object? sender, EditArchiveEventArgs args);

    public event EditArchiveEventHandler? EditArchive;

    #endregion Events

    #region Fields

    private readonly ApiClient _apiClient;
    private readonly UserModel _userModel;

    private BaseGroupModel? _selectedGroupModel;

    private GameModel? _selectedGameModel;
    private bool _isAllGamesChecked;

    private string? _filter;

    private bool? _isTitleOrdering;
    private bool? _isCountOrdering;
    private bool? _isBuyPriceOrdering;
    private bool? _isSoldPriceOrdering;
    private bool? _isSoldSumOrdering;
    private bool? _isChangeOrdering;

    private Archives.ArchiveOrderName? _archiveOrderName;
    private bool? _isAscending;

    private List<ArchiveViewModel> _archiveModels;
    private ArchiveViewModel? _selectedArchiveModel;

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
                ArchiveOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ArchiveOrderName = Archives.ArchiveOrderName.Title;
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
                ArchiveOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ArchiveOrderName = Archives.ArchiveOrderName.Count;
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
                ArchiveOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ArchiveOrderName = Archives.ArchiveOrderName.BuyPrice;
                IsAscending = value;
            }

            SetProperty(ref _isBuyPriceOrdering, value);
        }
    }

    public bool? IsSoldPriceOrdering
    {
        get => _isSoldPriceOrdering;
        set
        {
            if (_isSoldPriceOrdering is not null && value is null)
            {
                ArchiveOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ArchiveOrderName = Archives.ArchiveOrderName.SoldPrice;
                IsAscending = value;
            }

            SetProperty(ref _isSoldPriceOrdering, value);
        }
    }

    public bool? IsSoldSumOrdering
    {
        get => _isSoldSumOrdering;
        set
        {
            if (_isSoldSumOrdering is not null && value is null)
            {
                ArchiveOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ArchiveOrderName = Archives.ArchiveOrderName.SoldSum;
                IsAscending = value;
            }

            SetProperty(ref _isSoldSumOrdering, value);
        }
    }

    public bool? IsChangeOrdering
    {
        get => _isChangeOrdering;
        set
        {
            if (_isChangeOrdering is not null && value is null)
            {
                ArchiveOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ArchiveOrderName = Archives.ArchiveOrderName.Change;
                IsAscending = value;
            }

            SetProperty(ref _isChangeOrdering, value);
        }
    }

    public List<ArchiveViewModel> ArchiveModels
    {
        get => _archiveModels;
        private set
        {
            SetProperty(ref _archiveModels, value);
            OnPropertyChanged(nameof(NotFoundText));
        }
    }

    public ArchiveViewModel? SelectedArchiveModel
    {
        get => _selectedArchiveModel;
        set => SetProperty(ref _selectedArchiveModel, value);
    }

    public string? NotFoundText
    {
        get => ArchiveModels.Count == 0 && !IsLoading ? EMPTY_LIST_TEXT : null;
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

    private Archives.ArchiveOrderName? ArchiveOrderName
    {
        get => _archiveOrderName;
        set
        {
            SetProperty(ref _archiveOrderName, value);
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

    public RelayCommand<ArchiveModel> EditCommand { get; }

    public RelayCommand<ArchiveModel> DeleteCommand { get; }

    #endregion Commands

    #region Constructor

    public ListArchivesModel(
        ApiClient apiClient,
        UserModel userModel)
    {
        _apiClient = apiClient;
        _userModel = userModel;

        userModel.UserChanged += UserChangedHandler;
        userModel.CurrencyChanged += CurrencyChangedHandler;

        _archiveModels = [];
        _cancellationTokenSource = new();

        IsAllGamesChecked = true;

        SetOrderingsNull();

        IsLoading = false;

        PageNumber = 1;
        PageSize = 20;

        ClearFiltersCommand = new(DoClearFiltersCommand);
        EditCommand = new(DoEditCommand);
        DeleteCommand = new(DoDeleteCommand);
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
        SelectedGroupModel = null;
        IsAllGamesChecked = true;
        ArchiveOrderName = null;
        IsAscending = null;
        PageNumber = 1;
        SetOrderingsNull();
    }

    private void DoEditCommand(ArchiveModel? model)
    {
        OnEditArchive(model);
    }

    private void DoDeleteCommand(ArchiveModel? model)
    {

    }

    public void OpenArchiveGroup(ArchiveGroupModel? model)
    {

    }

    private void SetOrderingsNull()
    {
        IsTitleOrdering = null;
        IsCountOrdering = null;
        IsBuyPriceOrdering = null;
        IsSoldPriceOrdering = null;
        IsSoldSumOrdering = null;
        IsChangeOrdering = null;
    }

    private async void GetSkins()
    {
        ArchiveModels = [];
        if (_userModel.User is null) return;

        IsLoading = true;

        await CancellationTokenSource.CancelAsync();

        CancellationTokenSource = new();
        CancellationToken token = CancellationTokenSource.Token;

        CurrentPageNumber = PageNumber ?? 1;

        DisplayItemsCountStart = (CurrentPageNumber - 1) * PageSize + 1;
        DisplayItemsCountEnd = CurrentPageNumber * PageSize;

        Archives.ArchivesResponse? archivesResponse =
            await _apiClient.GetAsync<Archives.ArchivesResponse, Archives.GetArchivesRequest>(
                ApiConstants.ApiMethods.GetArchives,
                new(SelectedGroupModel?.GroupId, SelectedGameModel?.Id, Filter, ArchiveOrderName, IsAscending,
                    CurrentPageNumber, PageSize),
                token);

        if (archivesResponse is null) return;

        SavedItemsCount = archivesResponse.Count;
        PagesCount = archivesResponse.PagesCount;

        ArchiveModels = archivesResponse.Archives.Select(x =>
                new ArchiveViewModel(
                    new(x.Skin.Id,
                        x.Skin.SkinIconUrl,
                        x.Skin.MarketUrl,
                        x.Skin.Title,
                        x.Id,
                        x.GroupId,
                        x.Count,
                        x.BuyPrice,
                        x.SoldPrice,
                        x.SoldSum,
                        _userModel.CurrencyMark,
                        x.Change,
                        x.BuyDate,
                        x.SoldDate,
                        x.Description),
                    this))
            .ToList();

        IsLoading = false;
    }

    private void OnEditArchive(ArchiveModel? model)
    {
        EditArchive?.Invoke(this, new(model));
    }

    #endregion Methods
}
