using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorage.Services.ThemeService;
using SteamStorage.Utilities.Events;
using SteamStorage.ViewModels.UtilityViewModels;
using SteamStorageAPI;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Utilities;

namespace SteamStorage.Models;

public class ActivesReviewModel : ModelBase
{
    #region Constants

    private const string EMPTY_LIST_TEXT = "Элементы не найдены";

    #endregion Constants

    #region Fields

    private readonly ApiClient _apiClient;
    private readonly ActiveGroupsModel _activeGroupsModel;
    private readonly ChartTooltipModel _chartTooltipModel;
    private readonly UserModel _userModel;
    private readonly IThemeService _themeService;

    private int _count;
    private string _investedSumString;
    private string _currentSumString;

    private bool? _isTitleOrdering;
    private bool? _isCountOrdering;
    private bool? _isBuySumOrdering;
    private bool? _isCurrentSumOrdering;
    private bool? _isChangeOrdering;

    private ActiveGroups.ActiveGroupOrderName? _activeGroupOrderName;
    private bool? _isAscending;

    private List<ActiveGroupViewModel> _activeGroupModels;
    private ActiveGroupViewModel? _selectedActiveGroupModel;

    private bool _isLoading;
    private CancellationTokenSource _cancellationTokenSource;

    #endregion Fields

    #region Properties

    public int Count
    {
        get => _count;
        private set => SetProperty(ref _count, value);
    }

    public string InvestedSumString
    {
        get => _investedSumString;
        private set => SetProperty(ref _investedSumString, value);
    }

    public string CurrentSumString
    {
        get => _currentSumString;
        private set => SetProperty(ref _currentSumString, value);
    }

    public bool? IsTitleOrdering
    {
        get => _isTitleOrdering;
        set
        {
            if (_isTitleOrdering is not null && value is null)
            {
                ActiveGroupOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ActiveGroupOrderName = ActiveGroups.ActiveGroupOrderName.Title;
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
                ActiveGroupOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ActiveGroupOrderName = ActiveGroups.ActiveGroupOrderName.Count;
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
                ActiveGroupOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ActiveGroupOrderName = ActiveGroups.ActiveGroupOrderName.BuySum;
                IsAscending = value;
            }

            SetProperty(ref _isBuySumOrdering, value);
        }
    }

    public bool? IsCurrentSumOrdering
    {
        get => _isCurrentSumOrdering;
        set
        {
            if (_isCurrentSumOrdering is not null && value is null)
            {
                ActiveGroupOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ActiveGroupOrderName = ActiveGroups.ActiveGroupOrderName.CurrentSum;
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
                ActiveGroupOrderName = null;
                IsAscending = null;
            }

            if (value is not null)
            {
                SetOrderingsNull();
                ActiveGroupOrderName = ActiveGroups.ActiveGroupOrderName.Change;
                IsAscending = value;
            }

            SetProperty(ref _isChangeOrdering, value);
        }
    }

    public List<ActiveGroupViewModel> ActiveGroupModels
    {
        get => _activeGroupModels;
        private set
        {
            SetProperty(ref _activeGroupModels, value);
            OnPropertyChanged(nameof(NotFoundText));
        }
    }

    public ActiveGroupViewModel? SelectedActiveGroupModel
    {
        get => _selectedActiveGroupModel;
        set
        {
            SetProperty(ref _selectedActiveGroupModel, value);
            SelectedActiveGroupModel?.UpdateStats();
        }
    }

    public string? NotFoundText
    {
        get => ActiveGroupModels.Count == 0 && !IsLoading ? EMPTY_LIST_TEXT : null;
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

    private ActiveGroups.ActiveGroupOrderName? ActiveGroupOrderName
    {
        get => _activeGroupOrderName;
        set
        {
            SetProperty(ref _activeGroupOrderName, value);
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

    public ActivesReviewModel(
        ApiClient apiClient,
        ActiveGroupsModel activeGroupsModel,
        ChartTooltipModel chartTooltipModel,
        UserModel userModel,
        IThemeService themeService)
    {
        _apiClient = apiClient;
        _activeGroupsModel = activeGroupsModel;
        _chartTooltipModel = chartTooltipModel;
        _userModel = userModel;
        _themeService = themeService;

        userModel.UserChanged += UserChangedHandler;
        userModel.CurrencyChanged += CurrencyChangedHandler;

        themeService.ChartThemeChanged += ChartThemeChangedHandler;

        _investedSumString = string.Empty;
        _currentSumString = string.Empty;

        _activeGroupModels = [];
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
        IsCurrentSumOrdering = null;
        IsChangeOrdering = null;
    }

    private void DoAttachedToVisualTreeCommand()
    {
        RefreshStatistics();
        GetGroups();
        _activeGroupsModel.UpdateGroups();
    }

    private void RefreshStatistics()
    {

    }

    private async void GetGroups()
    {
        ActiveGroupModels = [];
        if (_userModel.User is null) return;

        IsLoading = true;

        await CancellationTokenSource.CancelAsync();

        CancellationTokenSource = new();
        CancellationToken token = CancellationTokenSource.Token;

        ActiveGroups.ActiveGroupsResponse? groupsResponse =
            await _apiClient.GetAsync<ActiveGroups.ActiveGroupsResponse, ActiveGroups.GetActiveGroupsRequest>(
                ApiConstants.ApiControllers.ActiveGroups,
                "GetActiveGroups",
                new(ActiveGroupOrderName, IsAscending),
                token);

        if (groupsResponse is null) return;

        ActiveGroupModels = groupsResponse.ActiveGroups.Select(x =>
                new ActiveGroupViewModel(
                    new(_apiClient, _themeService, x.Id, x.Colour, x.Title, x.Count, x.GoalSum, x.GoalSumCompletion,
                        x.BuySum, x.CurrentSum, _userModel.CurrencyMark, x.Change, x.DateCreation),
                    _activeGroupsModel,
                    _chartTooltipModel))
            .ToList();

        IsLoading = false;
    }

    #endregion Methods
}
