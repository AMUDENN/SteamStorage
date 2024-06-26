﻿using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using SteamStorage.Models.Tools;
using SteamStorage.Services.ThemeService;
using SteamStorage.Utilities.Events.Settings;
using SteamStorage.Utilities.ThemeVariants;
using SteamStorageAPI.SDK;
using SteamStorageAPI.SDK.ApiEntities;
using SteamStorageAPI.SDK.Services.Ping.PingResult;
using SteamStorageAPI.SDK.Services.Ping.PingService;
using SteamStorageAPI.SDK.Utilities;

namespace SteamStorage.Models.Home;

public class StatisticsModel : ModelBase
{
    #region Constnats

    private const double INVESTED_SUM_WIDTH_DEFAULT = 160;
    private const double FINANCIAL_GOAL_WIDTH_DEFAULT = 120;
    private const double INVENTORY_GAMES_WIDTH_DEFAULT = 200;

    #endregion Constants

    #region Fields

    private readonly ApiClient _apiClient;
    private readonly IPingService _pingService;
    private readonly UserModel _userModel;
    private readonly IThemeService _themeService;

    private string _investedSumString;
    private double _investedSumGrowth;
    private IEnumerable<ISeries> _investedSumGrowthSeries;
    private double _investedSumWidth;

    private string _financialGoalString;
    private double _financialGoalPercentageCompletion;
    private IEnumerable<ISeries> _financialGoalPercentageCompletionSeries;
    private double _financialGoalWidth;

    private int _totalCount;

    private int _activesCount;
    private string _activesCurrentSumString;
    private double _activesPercentageGrowth;

    private int _archivesCount;
    private string _archivesSoldSumString;
    private double _archivesPercentageGrowth;

    private int _inventoryCount;
    private string _inventorySumString;
    private IEnumerable<Statistics.InventoryGameStatisticResponse> _inventoryGames;
    private IEnumerable<ISeries> _inventoryGamesSeries;
    private double _inventoryGamesWidth;

    private long _ping;
    private PingResult.ServerStatus _status;

    #endregion Fields

    #region Properties

    public string InvestedSumString
    {
        get => _investedSumString;
        private set => SetProperty(ref _investedSumString, value);
    }

    public double InvestedSumGrowth
    {
        get => _investedSumGrowth;
        private set
        {
            SetProperty(ref _investedSumGrowth, value);
            GetInvestedSumGrowthSeries();
        }
    }

    public IEnumerable<ISeries> InvestedSumGrowthSeries
    {
        get => _investedSumGrowthSeries;
        private set => SetProperty(ref _investedSumGrowthSeries, value);
    }

    public double InvestedSumWidth
    {
        get => _investedSumWidth;
        private set => SetProperty(ref _investedSumWidth, value);
    }

    public string FinancialGoalString
    {
        get => _financialGoalString;
        private set => SetProperty(ref _financialGoalString, value);
    }

    public double FinancialGoalPercentageCompletion
    {
        get => _financialGoalPercentageCompletion;
        private set
        {
            SetProperty(ref _financialGoalPercentageCompletion, value);
            GetFinancialGoalPercentageCompletion();
        }
    }

    public IEnumerable<ISeries> FinancialGoalPercentageCompletionSeries
    {
        get => _financialGoalPercentageCompletionSeries;
        private set => SetProperty(ref _financialGoalPercentageCompletionSeries, value);
    }

    public double FinancialGoalWidth
    {
        get => _financialGoalWidth;
        private set => SetProperty(ref _financialGoalWidth, value);
    }

    public int TotalCount
    {
        get => _totalCount;
        private set => SetProperty(ref _totalCount, value);
    }

    public int ActivesCount
    {
        get => _activesCount;
        private set => SetProperty(ref _activesCount, value);
    }

    public string ActivesCurrentSumString
    {
        get => _activesCurrentSumString;
        private set => SetProperty(ref _activesCurrentSumString, value);
    }

    public double ActivesPercentageGrowth
    {
        get => _activesPercentageGrowth;
        private set => SetProperty(ref _activesPercentageGrowth, value);
    }

    public int ArchivesCount
    {
        get => _archivesCount;
        private set => SetProperty(ref _archivesCount, value);
    }

    public string ArchivesSoldSumString
    {
        get => _archivesSoldSumString;
        private set => SetProperty(ref _archivesSoldSumString, value);
    }

    public double ArchivesPercentageGrowth
    {
        get => _archivesPercentageGrowth;
        private set => SetProperty(ref _archivesPercentageGrowth, value);
    }

    public int InventoryCount
    {
        get => _inventoryCount;
        private set => SetProperty(ref _inventoryCount, value);
    }

    public string InventorySumString
    {
        get => _inventorySumString;
        private set => SetProperty(ref _inventorySumString, value);
    }

    private IEnumerable<Statistics.InventoryGameStatisticResponse> InventoryGames
    {
        get => _inventoryGames;
        set
        {
            SetProperty(ref _inventoryGames, value);
            GetInventoryGamesSeries();
        }
    }

    public IEnumerable<ISeries> InventoryGamesSeries
    {
        get => _inventoryGamesSeries;
        private set => SetProperty(ref _inventoryGamesSeries, value);
    }

    public double InventoryGamesWidth
    {
        get => _inventoryGamesWidth;
        private set => SetProperty(ref _inventoryGamesWidth, value);
    }

    public long Ping
    {
        get => _ping;
        private set => SetProperty(ref _ping, value);
    }

    private PingResult.ServerStatus Status
    {
        get => _status;
        set
        {
            SetProperty(ref _status, value);
            OnPropertyChanged(nameof(ServerStatusString));
            OnPropertyChanged(nameof(ServerStatusBool));
            OnPropertyChanged(nameof(IsServerActive));
        }
    }

    public string ServerStatusString
    {
        get
        {
            return Status switch
            {
                PingResult.ServerStatus.Excellent => "Отличное",
                PingResult.ServerStatus.Good => "Хорошее",
                PingResult.ServerStatus.Bad => "Плохое",
                PingResult.ServerStatus.NoConnection => "Нет соединения",
                _ => "Нет информации"
            };
        }
    }

    public bool? ServerStatusBool
    {
        get
        {
            return Status switch
            {
                PingResult.ServerStatus.Excellent => null,
                PingResult.ServerStatus.Good => true,
                PingResult.ServerStatus.Bad => false,
                PingResult.ServerStatus.NoConnection => false,
                _ => false
            };
        }
    }

    public bool IsServerActive
    {
        get
        {
            return Status switch
            {
                PingResult.ServerStatus.Excellent => true,
                PingResult.ServerStatus.Good => true,
                PingResult.ServerStatus.Bad => true,
                PingResult.ServerStatus.NoConnection => false,
                _ => false
            };
        }
    }

    #endregion Properties

    #region Commands

    public RelayCommand AttachedToVisualTreeCommand { get; }

    #endregion Commands

    #region Constructor

    public StatisticsModel(
        ApiClient apiClient,
        IPingService pingService,
        UserModel userModel,
        IThemeService themeService)
    {
        _apiClient = apiClient;
        _pingService = pingService;
        _userModel = userModel;
        _themeService = themeService;

        userModel.UserChanged += UserChangedHandler;
        userModel.CurrencyChanged += CurrencyChangedHandler;
        userModel.FinancialGoalChanged += FinancialGoalChangedHandler;

        themeService.ChartThemeChanged += ChartThemeChangedHandler;

        _investedSumString = string.Empty;
        _financialGoalString = string.Empty;
        _activesCurrentSumString = string.Empty;
        _archivesSoldSumString = string.Empty;
        _inventorySumString = string.Empty;

        _investedSumGrowthSeries = Enumerable.Empty<ISeries>();
        _investedSumWidth = INVESTED_SUM_WIDTH_DEFAULT;
        _financialGoalPercentageCompletionSeries = Enumerable.Empty<ISeries>();
        _financialGoalWidth = FINANCIAL_GOAL_WIDTH_DEFAULT;

        _inventoryGames = Enumerable.Empty<Statistics.InventoryGameStatisticResponse>();
        _inventoryGamesSeries = Enumerable.Empty<ISeries>();
        _inventoryGamesWidth = INVENTORY_GAMES_WIDTH_DEFAULT;

        AttachedToVisualTreeCommand = new(DoAttachedToVisualTreeCommand);

        RefreshPingAsync();
    }

    #endregion Constructor

    #region Methods

    private void UserChangedHandler(object? sender)
    {
        RefreshStatisticsAsync();
        RefreshPingAsync();
    }

    private void CurrencyChangedHandler(object? sender)
    {
        RefreshStatisticsAsync();
    }

    private void FinancialGoalChangedHandler(object? sender)
    {
        RefreshStatisticsAsync();
    }

    private void ChartThemeChangedHandler(object? sender, ChartThemeChangedEventArgs args)
    {
        GetInventoryGamesSeries();
        GetInvestedSumGrowthSeries();
        GetFinancialGoalPercentageCompletion();
    }

    private void DoAttachedToVisualTreeCommand()
    {
        RefreshStatisticsAsync();
        RefreshPingAsync();
    }

    private void GetInvestedSumGrowthSeries()
    {
        double growth = InvestedSumGrowth < 0 ? 0 : InvestedSumGrowth > 1 ? 100 : InvestedSumGrowth * 100;

        InvestedSumGrowthSeries = GaugeGenerator.BuildSolidGauge(
            new GaugeItem(
                growth,
                series =>
                {
                    series.MaxRadialColumnWidth = 20;
                    series.Fill = new SolidColorPaint(_themeService.CurrentChartThemeVariant
                        .GetChartColor(ChartThemeVariants.ChartColors.SecondAccent).Color);
                    series.DataLabelsSize = 0;
                }),
            new GaugeItem(GaugeItem.Background, series =>
            {
                series.MaxRadialColumnWidth = 20;
                series.Fill = new SolidColorPaint(_themeService.CurrentChartThemeVariant
                    .GetChartColor(ChartThemeVariants.ChartColors.ThirdAccent).Color);
            }));

        InvestedSumWidth = InvestedSumWidth < INVESTED_SUM_WIDTH_DEFAULT 
            ? InvestedSumWidth + 1 
            : InvestedSumWidth - 1;
    }

    private void GetFinancialGoalPercentageCompletion()
    {
        double growth = FinancialGoalPercentageCompletion < 0 ? 0 :
            FinancialGoalPercentageCompletion > 1 ? 100 : FinancialGoalPercentageCompletion * 100;

        FinancialGoalPercentageCompletionSeries = GaugeGenerator.BuildSolidGauge(
            new GaugeItem(
                growth,
                series =>
                {
                    series.MaxRadialColumnWidth = 20;
                    series.Fill = new SolidColorPaint(_themeService.CurrentChartThemeVariant
                        .GetChartColor(ChartThemeVariants.ChartColors.FirstAccent).Color);
                    series.DataLabelsSize = 0;
                }),
            new GaugeItem(GaugeItem.Background, series =>
            {
                series.MaxRadialColumnWidth = 20;
                series.Fill = new SolidColorPaint(_themeService.CurrentChartThemeVariant
                    .GetChartColor(ChartThemeVariants.ChartColors.ThirdAccent).Color);
            }));

        FinancialGoalWidth = FinancialGoalWidth < FINANCIAL_GOAL_WIDTH_DEFAULT
            ? FinancialGoalWidth + 1
            : FinancialGoalWidth - 1;
    }

    private void GetInventoryGamesSeries()
    {
        if (!InventoryGames.Any()) return;

        int i = 0;
        InventoryGamesSeries = InventoryGames.OrderByDescending(x => x.GameTitle)
            .AsPieSeries((value, builder) =>
            {
                builder.MaxRadialColumnWidth = 20;
                builder.HoverPushout = 0;
                builder.Mapping = (game, point) => new(point, game.Count);
                builder.ToolTipLabelFormatter = _ => $"{value.GameTitle}: {value.Count:N0}";
                builder.Fill = new SolidColorPaint(_themeService.CurrentChartThemeVariant.Colors.ElementAt(i).Color);
                i++;
            });

        InventoryGamesWidth = InventoryGamesWidth < INVENTORY_GAMES_WIDTH_DEFAULT
            ? InventoryGamesWidth + 1
            : InventoryGamesWidth - 1;
    }

    private async void RefreshStatisticsAsync()
    {
        Statistics.InvestmentSumResponse? investmentSumResponse =
            await _apiClient.GetAsync<Statistics.InvestmentSumResponse>(
                ApiConstants.ApiMethods.GetInvestmentSum);

        InvestedSumString = $"{investmentSumResponse?.TotalSum ?? 0:N2} {_userModel.CurrencyMark}";
        InvestedSumGrowth = investmentSumResponse?.PercentageGrowth ?? 0;


        Statistics.FinancialGoalResponse? financialGoalResponse =
            await _apiClient.GetAsync<Statistics.FinancialGoalResponse>(
                ApiConstants.ApiMethods.GetFinancialGoal);

        FinancialGoalString = $"{financialGoalResponse?.FinancialGoal ?? 0:N0} {_userModel.CurrencyMark}";
        FinancialGoalPercentageCompletion = financialGoalResponse?.PercentageCompletion ?? 0;


        Statistics.ItemsCountResponse? itemsCountResponse =
            await _apiClient.GetAsync<Statistics.ItemsCountResponse>(
                ApiConstants.ApiMethods.GetItemsCount);

        TotalCount = itemsCountResponse?.Count ?? 0;


        Statistics.ActiveStatisticResponse? activeStatisticResponse =
            await _apiClient.GetAsync<Statistics.ActiveStatisticResponse>(
                ApiConstants.ApiMethods.GetActiveStatistic);

        ActivesCount = activeStatisticResponse?.Count ?? 0;
        ActivesCurrentSumString = $"{activeStatisticResponse?.CurrentSum ?? 0:N2} {_userModel.CurrencyMark}";
        ActivesPercentageGrowth = activeStatisticResponse?.PercentageGrowth ?? 0;


        Statistics.ArchiveStatisticResponse? archiveStatisticResponse =
            await _apiClient.GetAsync<Statistics.ArchiveStatisticResponse>(
                ApiConstants.ApiMethods.GetArchiveStatistic);

        ArchivesCount = archiveStatisticResponse?.Count ?? 0;
        ArchivesSoldSumString = $"{archiveStatisticResponse?.SoldSum ?? 0:N2} {_userModel.CurrencyMark}";
        ArchivesPercentageGrowth = archiveStatisticResponse?.PercentageGrowth ?? 0;


        Statistics.InventoryStatisticResponse? inventoryStatisticResponse =
            await _apiClient.GetAsync<Statistics.InventoryStatisticResponse>(
                ApiConstants.ApiMethods.GetInventoryStatistic);

        InventoryCount = inventoryStatisticResponse?.Count ?? 0;
        InventorySumString = $"{inventoryStatisticResponse?.Sum ?? 0:N2} {_userModel.CurrencyMark}";
        InventoryGames = inventoryStatisticResponse?.Games ??
                         Enumerable.Empty<Statistics.InventoryGameStatisticResponse>();
    }

    private async void RefreshPingAsync()
    {
        PingResult pingResult = await _pingService.GetPing();
        Ping = pingResult.Ping;
        Status = pingResult.Status;
    }

    #endregion Methods
}
