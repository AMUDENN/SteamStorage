using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using SteamStorage.Models.Tools;
using SteamStorage.Services.ThemeService;
using SteamStorage.Utilities.Events.Settings;
using SteamStorage.ViewModels.UtilityViewModels;
using SteamStorageAPI.SDK;
using SteamStorageAPI.SDK.ApiEntities;
using SteamStorageAPI.SDK.Utilities;

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
    private IEnumerable<ActiveGroups.ActiveGroupsGameCountResponse> _activeGroupsGameCount;
    private IEnumerable<ISeries> _activeGroupsGameCountSeries;
    private IEnumerable<ActiveGroups.ActiveGroupsGameInvestmentSumResponse> _activeGroupsGameInvestmentSum;
    private IEnumerable<ISeries> _activeGroupsGameInvestmentSumSeries;
    private IEnumerable<ActiveGroups.ActiveGroupsGameCurrentSumResponse> _activeGroupsGameCurrentSum;
    private IEnumerable<ISeries> _activeGroupsGameCurrentSumSeries;

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
    private CancellationTokenSource _groupsCancellationTokenSource;
    private CancellationTokenSource _statisticsCancellationTokenSource;

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

    private IEnumerable<ActiveGroups.ActiveGroupsGameCountResponse> ActiveGroupsGameCount
    {
        get => _activeGroupsGameCount;
        set
        {
            SetProperty(ref _activeGroupsGameCount, value);
            GetActiveGroupsGameCountSeries();
        }
    }

    public IEnumerable<ISeries> ActiveGroupsGameCountSeries
    {
        get => _activeGroupsGameCountSeries;
        private set => SetProperty(ref _activeGroupsGameCountSeries, value);
    }

    private IEnumerable<ActiveGroups.ActiveGroupsGameInvestmentSumResponse> ActiveGroupsGameInvestmentSum
    {
        get => _activeGroupsGameInvestmentSum;
        set
        {
            SetProperty(ref _activeGroupsGameInvestmentSum, value);
            GetActiveGroupsGameInvestmentSumSeries();
        }
    }

    public IEnumerable<ISeries> ActiveGroupsGameInvestmentSumSeries
    {
        get => _activeGroupsGameInvestmentSumSeries;
        private set => SetProperty(ref _activeGroupsGameInvestmentSumSeries, value);
    }

    private IEnumerable<ActiveGroups.ActiveGroupsGameCurrentSumResponse> ActiveGroupsGameCurrentSum
    {
        get => _activeGroupsGameCurrentSum;
        set
        {
            SetProperty(ref _activeGroupsGameCurrentSum, value);
            GetActiveGroupsGameCurrentSumSeries();
        }
    }

    public IEnumerable<ISeries> ActiveGroupsGameCurrentSumSeries
    {
        get => _activeGroupsGameCurrentSumSeries;
        private set => SetProperty(ref _activeGroupsGameCurrentSumSeries, value);
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
            GetGroupsAsync();
        }
    }

    private bool? IsAscending
    {
        get => _isAscending;
        set
        {
            SetProperty(ref _isAscending, value);
            GetGroupsAsync();
        }
    }

    private CancellationTokenSource GroupsCancellationTokenSource
    {
        get => _groupsCancellationTokenSource;
        set => SetProperty(ref _groupsCancellationTokenSource, value);
    }

    private CancellationTokenSource StatisticsCancellationTokenSource
    {
        get => _statisticsCancellationTokenSource;
        set => SetProperty(ref _statisticsCancellationTokenSource, value);
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

        _activeGroupsGameCount = Enumerable.Empty<ActiveGroups.ActiveGroupsGameCountResponse>();
        _activeGroupsGameCountSeries = Enumerable.Empty<ISeries>();
        _activeGroupsGameInvestmentSum = Enumerable.Empty<ActiveGroups.ActiveGroupsGameInvestmentSumResponse>();
        _activeGroupsGameInvestmentSumSeries = Enumerable.Empty<ISeries>();
        _activeGroupsGameCurrentSum = Enumerable.Empty<ActiveGroups.ActiveGroupsGameCurrentSumResponse>();
        _activeGroupsGameCurrentSumSeries = Enumerable.Empty<ISeries>();

        _activeGroupModels = [];
        _groupsCancellationTokenSource = new();
        _statisticsCancellationTokenSource = new();

        SetOrderingsNull();

        IsLoading = false;

        AttachedToVisualTreeCommand = new(DoAttachedToVisualTreeCommand);
    }

    #endregion Constructor

    #region Methods

    private void UserChangedHandler(object? sender)
    {
        RefreshStatisticsAsync();
        GetGroupsAsync();
    }

    private void CurrencyChangedHandler(object? sender)
    {
        RefreshStatisticsAsync();
        GetGroupsAsync();
    }

    private void ChartThemeChangedHandler(object? sender, ChartThemeChangedEventArgs args)
    {
        GetActiveGroupsGameCountSeries();
        GetActiveGroupsGameInvestmentSumSeries();
        GetActiveGroupsGameCurrentSumSeries();
    }

    private void DoAttachedToVisualTreeCommand()
    {
        RefreshStatisticsAsync();
        GetGroupsAsync();
        _activeGroupsModel.UpdateGroups();
    }

    public void UpdateGroups()
    {
        RefreshStatisticsAsync();
        GetGroupsAsync();
    }

    private void SetOrderingsNull()
    {
        IsTitleOrdering = null;
        IsCountOrdering = null;
        IsBuySumOrdering = null;
        IsCurrentSumOrdering = null;
        IsChangeOrdering = null;
    }

    private void GetActiveGroupsGameCountSeries()
    {
        if (!ActiveGroupsGameCount.Any()) return;

        int i = 0;

        ActiveGroupsGameCountSeries = ActiveGroupsGameCount.OrderByDescending(x => x.Count)
            .AsPieSeries((value, builder) =>
            {
                builder.MaxRadialColumnWidth = 20;
                builder.HoverPushout = 0;
                builder.Mapping = (game, point) => new(point, game.Count);
                builder.ToolTipLabelFormatter = _ => $"{value.GameTitle}: {value.Count:N0}";
                builder.Fill = new SolidColorPaint(_themeService.CurrentChartThemeVariant.Colors.ElementAt(i).Color);
                i++;
            });
    }

    private void GetActiveGroupsGameInvestmentSumSeries()
    {
        if (!ActiveGroupsGameInvestmentSum.Any()) return;

        int i = 0;

        ActiveGroupsGameInvestmentSumSeries = ActiveGroupsGameInvestmentSum.OrderByDescending(x => x.InvestmentSum)
            .AsPieSeries((value, builder) =>
            {
                builder.MaxRadialColumnWidth = 20;
                builder.HoverPushout = 0;
                builder.Mapping = (game, point) => new(point, (double)game.InvestmentSum);
                builder.ToolTipLabelFormatter = _ => $"{value.GameTitle}: {value.InvestmentSum:N2}";
                builder.Fill = new SolidColorPaint(_themeService.CurrentChartThemeVariant.Colors.ElementAt(i).Color);
                i++;
            });
    }

    private void GetActiveGroupsGameCurrentSumSeries()
    {
        if (!ActiveGroupsGameCurrentSum.Any()) return;

        int i = 0;

        ActiveGroupsGameCurrentSumSeries = ActiveGroupsGameCurrentSum.OrderByDescending(x => x.CurrentSum)
            .AsPieSeries((value, builder) =>
            {
                builder.MaxRadialColumnWidth = 20;
                builder.HoverPushout = 0;
                builder.Mapping = (game, point) => new(point, (double)game.CurrentSum);
                builder.ToolTipLabelFormatter = _ => $"{value.GameTitle}: {value.CurrentSum:N2}";
                builder.Fill = new SolidColorPaint(_themeService.CurrentChartThemeVariant.Colors.ElementAt(i).Color);
                i++;
            });
    }

    private async void RefreshStatisticsAsync()
    {
        await StatisticsCancellationTokenSource.CancelAsync();

        StatisticsCancellationTokenSource = new();
        CancellationToken token = StatisticsCancellationTokenSource.Token;

        ActiveGroups.ActiveGroupsStatisticResponse? activeGroupsStatisticResponse =
            await _apiClient.GetAsync<ActiveGroups.ActiveGroupsStatisticResponse>(
                ApiConstants.ApiMethods.GetActiveGroupsStatistic,
                token);

        if (activeGroupsStatisticResponse is null) return;

        Count = activeGroupsStatisticResponse.ActivesCount;
        InvestedSumString = $"{activeGroupsStatisticResponse.InvestmentSum:N2} {_userModel.CurrencyMark}";
        CurrentSumString = $"{activeGroupsStatisticResponse.CurrentSum:N2} {_userModel.CurrencyMark}";

        if (activeGroupsStatisticResponse.GameCount is not null)
            ActiveGroupsGameCount = activeGroupsStatisticResponse.GameCount;

        if (activeGroupsStatisticResponse.GameInvestmentSum is not null)
            ActiveGroupsGameInvestmentSum = activeGroupsStatisticResponse.GameInvestmentSum;

        if (activeGroupsStatisticResponse.GameCurrentSum is not null)
            ActiveGroupsGameCurrentSum = activeGroupsStatisticResponse.GameCurrentSum;
    }

    private async void GetGroupsAsync()
    {
        ActiveGroupModels = [];
        if (_userModel.User is null) return;

        IsLoading = true;

        await GroupsCancellationTokenSource.CancelAsync();

        GroupsCancellationTokenSource = new();
        CancellationToken token = GroupsCancellationTokenSource.Token;

        ActiveGroups.ActiveGroupsResponse? groupsResponse =
            await _apiClient.GetAsync<ActiveGroups.ActiveGroupsResponse, ActiveGroups.GetActiveGroupsRequest>(
                ApiConstants.ApiMethods.GetActiveGroups,
                new(ActiveGroupOrderName, IsAscending),
                token);

        if (groupsResponse?.ActiveGroups is null) return;

        ActiveGroupModels = groupsResponse.ActiveGroups.Select(x =>
                new ActiveGroupViewModel(
                    new(_apiClient,
                        _themeService,
                        x.Id,
                        x.Colour,
                        x.Title,
                        x.Count,
                        x.GoalSum,
                        x.GoalSumCompletion,
                        x.BuySum,
                        x.CurrentSum,
                        _userModel.CurrencyMark,
                        x.Change,
                        x.DateCreation,
                        x.Description),
                    _activeGroupsModel,
                    _chartTooltipModel))
            .ToList();

        IsLoading = false;
    }

    #endregion Methods
}
