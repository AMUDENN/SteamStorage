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
    private readonly IThemeService _themeService;

    private int _count;
    private string _buySumString;
    private string _soldSumString;
    private IEnumerable<ArchiveGroups.ArchiveGroupsGameCountResponse> _archiveGroupsGameCount;
    private IEnumerable<ISeries> _archiveGroupsGameCountSeries;
    private IEnumerable<ArchiveGroups.ArchiveGroupsGameBuySumResponse> _archiveGroupsGameBuySum;
    private IEnumerable<ISeries> _archiveGroupsGameBuySumSeries;
    private IEnumerable<ArchiveGroups.ArchiveGroupsGameSoldSumResponse> _archiveGroupsGameSoldSum;
    private IEnumerable<ISeries> _archiveGroupsGameSoldSumSeries;

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
    private CancellationTokenSource _groupsCancellationTokenSource;
    private CancellationTokenSource _statisticsCancellationTokenSource;

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
    
    private IEnumerable<ArchiveGroups.ArchiveGroupsGameCountResponse> ArchiveGroupsGameCount
    {
        get => _archiveGroupsGameCount;
        set
        {
            SetProperty(ref _archiveGroupsGameCount, value);
            GetActiveGroupsGameCountSeries();
        }
    }

    public IEnumerable<ISeries> ArchiveGroupsGameCountSeries
    {
        get => _archiveGroupsGameCountSeries;
        private set => SetProperty(ref _archiveGroupsGameCountSeries, value);
    }

    private IEnumerable<ArchiveGroups.ArchiveGroupsGameBuySumResponse> ArchiveGroupsGameBuySum
    {
        get => _archiveGroupsGameBuySum;
        set
        {
            SetProperty(ref _archiveGroupsGameBuySum, value);
            GetActiveGroupsGameInvestmentSumSeries();
        }
    }

    public IEnumerable<ISeries> ArchiveGroupsGameBuySumSeries
    {
        get => _archiveGroupsGameBuySumSeries;
        private set => SetProperty(ref _archiveGroupsGameBuySumSeries, value);
    }

    private IEnumerable<ArchiveGroups.ArchiveGroupsGameSoldSumResponse> ArchiveGroupsGameSoldSum
    {
        get => _archiveGroupsGameSoldSum;
        set
        {
            SetProperty(ref _archiveGroupsGameSoldSum, value);
            GetActiveGroupsGameCurrentSumSeries();
        }
    }

    public IEnumerable<ISeries> ArchiveGroupsGameSoldSumSeries
    {
        get => _archiveGroupsGameSoldSumSeries;
        private set => SetProperty(ref _archiveGroupsGameSoldSumSeries, value);
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

    public ArchivesReviewModel(
        ApiClient apiClient,
        ArchiveGroupsModel archiveGroupsModel,
        UserModel userModel,
        IThemeService themeService)
    {
        _apiClient = apiClient;
        _archiveGroupsModel = archiveGroupsModel;
        _userModel = userModel;
        _themeService = themeService;

        userModel.UserChanged += UserChangedHandler;
        userModel.CurrencyChanged += CurrencyChangedHandler;

        themeService.ChartThemeChanged += ChartThemeChangedHandler;

        _buySumString = string.Empty;
        _soldSumString = string.Empty;
        
        _archiveGroupsGameCount = Enumerable.Empty<ArchiveGroups.ArchiveGroupsGameCountResponse>();
        _archiveGroupsGameCountSeries = Enumerable.Empty<ISeries>();
        _archiveGroupsGameBuySum = Enumerable.Empty<ArchiveGroups.ArchiveGroupsGameBuySumResponse>();
        _archiveGroupsGameBuySumSeries = Enumerable.Empty<ISeries>();
        _archiveGroupsGameSoldSum = Enumerable.Empty<ArchiveGroups.ArchiveGroupsGameSoldSumResponse>();
        _archiveGroupsGameSoldSumSeries = Enumerable.Empty<ISeries>();

        _archiveGroupModels = [];
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
        _archiveGroupsModel.UpdateGroups();
    }

    private void SetOrderingsNull()
    {
        IsTitleOrdering = null;
        IsCountOrdering = null;
        IsBuySumOrdering = null;
        IsSoldSumOrdering = null;
        IsChangeOrdering = null;
    }
    
    private void GetActiveGroupsGameCountSeries()
    {
        if (!ArchiveGroupsGameCount.Any()) return;

        int i = 0;

        ArchiveGroupsGameCountSeries = ArchiveGroupsGameCount.OrderByDescending(x => x.Count)
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
        if (!ArchiveGroupsGameBuySum.Any()) return;

        int i = 0;

        ArchiveGroupsGameBuySumSeries = ArchiveGroupsGameBuySum.OrderByDescending(x => x.BuySum)
            .AsPieSeries((value, builder) =>
            {
                builder.MaxRadialColumnWidth = 20;
                builder.HoverPushout = 0;
                builder.Mapping = (game, point) => new(point, (double)game.BuySum);
                builder.ToolTipLabelFormatter = _ => $"{value.GameTitle}: {value.BuySum:N2}";
                builder.Fill = new SolidColorPaint(_themeService.CurrentChartThemeVariant.Colors.ElementAt(i).Color);
                i++;
            });
    }

    private void GetActiveGroupsGameCurrentSumSeries()
    {
        if (!ArchiveGroupsGameSoldSum.Any()) return;

        int i = 0;

        ArchiveGroupsGameSoldSumSeries = ArchiveGroupsGameSoldSum.OrderByDescending(x => x.SoldSum)
            .AsPieSeries((value, builder) =>
            {
                builder.MaxRadialColumnWidth = 20;
                builder.HoverPushout = 0;
                builder.Mapping = (game, point) => new(point, (double)game.SoldSum);
                builder.ToolTipLabelFormatter = _ => $"{value.GameTitle}: {value.SoldSum:N2}";
                builder.Fill = new SolidColorPaint(_themeService.CurrentChartThemeVariant.Colors.ElementAt(i).Color);
                i++;
            });
    }
    
    private async void RefreshStatisticsAsync()
    {
        await StatisticsCancellationTokenSource.CancelAsync();

        StatisticsCancellationTokenSource = new();
        CancellationToken token = StatisticsCancellationTokenSource.Token;

        ArchiveGroups.ArchiveGroupsStatisticResponse? archiveGroupsStatisticResponse =
            await _apiClient.GetAsync<ArchiveGroups.ArchiveGroupsStatisticResponse>(
                ApiConstants.ApiMethods.GetArchiveGroupsStatistic,
                token);

        if (archiveGroupsStatisticResponse is null) return;

        Count = archiveGroupsStatisticResponse.ArchivesCount;
        BuySumString = $"{archiveGroupsStatisticResponse.BuySum:N2} {_userModel.CurrencyMark}";
        SoldSumString = $"{archiveGroupsStatisticResponse.SoldSum:N2} {_userModel.CurrencyMark}";

        if (archiveGroupsStatisticResponse.GameCount is not null)
            ArchiveGroupsGameCount = archiveGroupsStatisticResponse.GameCount;

        if (archiveGroupsStatisticResponse.GameBuySum is not null)
            ArchiveGroupsGameBuySum = archiveGroupsStatisticResponse.GameBuySum;

        if (archiveGroupsStatisticResponse.GameSoldSum is not null)
            ArchiveGroupsGameSoldSum = archiveGroupsStatisticResponse.GameSoldSum;
    }

    private async void GetGroupsAsync()
    {
        ArchiveGroupModels = [];
        if (_userModel.User is null) return;

        IsLoading = true;

        await GroupsCancellationTokenSource.CancelAsync();

        GroupsCancellationTokenSource = new();
        CancellationToken token = GroupsCancellationTokenSource.Token;

        ArchiveGroups.ArchiveGroupsResponse? groupsResponse =
            await _apiClient.GetAsync<ArchiveGroups.ArchiveGroupsResponse, ArchiveGroups.GetArchiveGroupsRequest>(
                ApiConstants.ApiMethods.GetArchiveGroups,
                new(ArchiveGroupOrderName, IsAscending),
                token);

        if (groupsResponse?.ArchiveGroups is null) return;

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
