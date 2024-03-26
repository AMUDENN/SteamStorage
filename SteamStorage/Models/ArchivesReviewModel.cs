using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorage.Services.ThemeService;
using SteamStorage.Utilities.Events.Settings;
using SteamStorage.ViewModels.UtilityViewModels;
using SteamStorageAPI;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Utilities;

namespace SteamStorage.Models;

public class ArchivesReviewModel : ModelBase
{
    #region Constants

    private const string EMPTY_LIST_TEXT = "Элементы не найдены";

    #endregion Constants

    #region Fields

    private readonly ApiClient _apiClient;
    private readonly ArchiveGroupsModel _archiveGroupsModel;
    private readonly UserModel _userModel;

    private int _count;
    private string _buySumString;
    private string _soldSumString;

    private bool? _isTitleOrdering;
    private bool? _isCountOrdering;
    private bool? _isBuySumOrdering;
    private bool? _isSoldSumOrdering;
    private bool? _isChangeOrdering;

    private ArchiveGroups.ArchiveGroupOrderName? _archiveGroupOrderName;
    private bool? _isAscending;

    private List<ArchiveGroupViewModel> _archiveGroupModels;
    private ArchiveGroupViewModel? _selectedArchiveGroupModel;

    private bool _isLoading;
    private CancellationTokenSource _cancellationTokenSource;

    #endregion Fields

    #region Properties

    public int Count
    {
        get => _count;
        private set => SetProperty(ref _count, value);
    }

    public string BuySumString
    {
        get => _buySumString;
        private set => SetProperty(ref _buySumString, value);
    }

    public string SoldSumString
    {
        get => _soldSumString;
        private set => SetProperty(ref _soldSumString, value);
    }

    public bool? IsTitleOrdering
    {
        get => _isTitleOrdering;
        set
        {
            if (_isTitleOrdering is not null && value is null)
            {
                ArchiveGroupOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ArchiveGroupOrderName = ArchiveGroups.ArchiveGroupOrderName.Title;
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
                ArchiveGroupOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ArchiveGroupOrderName = ArchiveGroups.ArchiveGroupOrderName.Count;
                IsAscending = value;
            }

            SetProperty(ref _isCountOrdering, value);
        }
    }

    public bool? IsBuySumOrdering
    {
        get => _isBuySumOrdering;
        set
        {
            if (_isBuySumOrdering is not null && value is null)
            {
                ArchiveGroupOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ArchiveGroupOrderName = ArchiveGroups.ArchiveGroupOrderName.BuySum;
                IsAscending = value;
            }

            SetProperty(ref _isBuySumOrdering, value);
        }
    }

    public bool? IsSoldSumOrdering
    {
        get => _isSoldSumOrdering;
        set
        {
            if (_isSoldSumOrdering is not null && value is null)
            {
                ArchiveGroupOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ArchiveGroupOrderName = ArchiveGroups.ArchiveGroupOrderName.SoldSum;
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
                ArchiveGroupOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ArchiveGroupOrderName = ArchiveGroups.ArchiveGroupOrderName.Change;
                IsAscending = value;
            }

            SetProperty(ref _isChangeOrdering, value);
        }
    }

    public List<ArchiveGroupViewModel> ArchiveGroupModels
    {
        get => _archiveGroupModels;
        private set
        {
            SetProperty(ref _archiveGroupModels, value);
            OnPropertyChanged(nameof(NotFoundText));
        }
    }

    public ArchiveGroupViewModel? SelectedArchiveGroupModel
    {
        get => _selectedArchiveGroupModel;
        set => SetProperty(ref _selectedArchiveGroupModel, value);
    }

    public string? NotFoundText
    {
        get => ArchiveGroupModels.Count == 0 && !IsLoading ? EMPTY_LIST_TEXT : null;
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

    private ArchiveGroups.ArchiveGroupOrderName? ArchiveGroupOrderName
    {
        get => _archiveGroupOrderName;
        set
        {
            SetProperty(ref _archiveGroupOrderName, value);
            GetGroups();
        }
    }

    private bool? IsAscending
    {
        get => _isAscending;
        set
        {
            SetProperty(ref _isAscending, value);
            GetGroups();
        }
    }

    private CancellationTokenSource CancellationTokenSource
    {
        get => _cancellationTokenSource;
        set => SetProperty(ref _cancellationTokenSource, value);
    }

    #endregion Properties

    #region Commands

    public RelayCommand AttachedToVisualTreeCommand { get; }

    #endregion Commands

    #region Constructor

    public ArchivesReviewModel(
        ApiClient apiClient,
        ArchiveGroupsModel archiveGroupsModel,
        UserModel userModel,
        IThemeService themeService)
    {
        _apiClient = apiClient;
        _archiveGroupsModel = archiveGroupsModel;
        _userModel = userModel;

        userModel.UserChanged += UserChangedHandler;
        userModel.CurrencyChanged += CurrencyChangedHandler;

        themeService.ChartThemeChanged += ChartThemeChangedHandler;

        _buySumString = string.Empty;
        _soldSumString = string.Empty;

        _archiveGroupModels = [];
        _cancellationTokenSource = new();

        SetOrderingsNull();

        IsLoading = false;

        AttachedToVisualTreeCommand = new(DoAttachedToVisualTreeCommand);
    }

    #endregion Constructor

    #region Methods

    private void UserChangedHandler(object? sender)
    {
        RefreshStatistics();
        GetGroups();
    }

    private void CurrencyChangedHandler(object? sender)
    {
        RefreshStatistics();
        GetGroups();
    }

    private void ChartThemeChangedHandler(object? sender, ChartThemeChangedEventArgs args)
    {
        //RefreshSeries
    }

    private void SetOrderingsNull()
    {
        IsTitleOrdering = null;
        IsCountOrdering = null;
        IsBuySumOrdering = null;
        IsSoldSumOrdering = null;
        IsChangeOrdering = null;
    }

    private void DoAttachedToVisualTreeCommand()
    {
        RefreshStatistics();
        GetGroups();
        _archiveGroupsModel.UpdateGroups();
    }

    private void RefreshStatistics()
    {

    }

    private async void GetGroups()
    {
        ArchiveGroupModels = [];
        if (_userModel.User is null) return;

        IsLoading = true;

        await CancellationTokenSource.CancelAsync();

        CancellationTokenSource = new();
        CancellationToken token = CancellationTokenSource.Token;

        ArchiveGroups.ArchiveGroupsResponse? groupsResponse =
            await _apiClient.GetAsync<ArchiveGroups.ArchiveGroupsResponse, ArchiveGroups.GetArchiveGroupsRequest>(
                ApiConstants.ApiMethods.GetArchiveGroups,
                new(ArchiveGroupOrderName, IsAscending),
                token);

        if (groupsResponse is null) return;

        ArchiveGroupModels = groupsResponse.ArchiveGroups.Select(x =>
                new ArchiveGroupViewModel(
                    new(x.Id, 
                        x.Colour, 
                        x.Title, 
                        x.Count, 
                        x.BuySum, 
                        x.SoldSum, 
                        _userModel.CurrencyMark, 
                        x.Change,
                        x.DateCreation,
                        x.Description),
                    _archiveGroupsModel))
            .ToList();

        IsLoading = false;
    }

    #endregion Methods
}
