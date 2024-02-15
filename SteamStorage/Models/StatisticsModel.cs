﻿using System;
using System.Collections.Generic;
using System.Linq;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using SteamStorage.Models.Tools;
using SteamStorage.Services.ThemeService;
using SteamStorage.Utilities.Events;
using SteamStorageAPI;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Services.PingResult;
using SteamStorageAPI.Utilities;

namespace SteamStorage.Models;

public class StatisticsModel : ModelBase
{
    #region Fields

    private readonly ApiClient _apiClient;
    private readonly IThemeService _themeService;

    private double _investedSum;
    private double _investedSumGrowth;
    private IEnumerable<ISeries> _investedSumGrowthSeries;

    private double _financialGoal;
    private double _financialGoalPercentageCompletion;

    private int _totalCount;

    private int _activesCount;
    private double _activesCurrentSum;
    private double _activesPercentageGrowth;

    private int _archivesCount;
    private double _archivesSoldSum;
    private double _archivesPercentageGrowth;


    private int _inventoryCount;
    private double _inventorySum;
    private IEnumerable<Statistics.InventoryGameStatisticResponse> _inventoryGames;
    private IEnumerable<ISeries> _inventoryGamesSeries;

    private long _ping;
    private PingResult.ServerStatus _status;

    #endregion Fields

    #region Properties

    public double InvestedSum
    {
        get => _investedSum;
        private set => SetProperty(ref _investedSum, value);
    }

    private double InvestedSumGrowth
    {
        get => _investedSumGrowth;
        set
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

    public double FinancialGoal
    {
        get => _financialGoal;
        private set => SetProperty(ref _financialGoal, value);
    }

    public double FinancialGoalPercentageCompletion
    {
        get => _financialGoalPercentageCompletion;
        private set => SetProperty(ref _financialGoalPercentageCompletion, value);
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

    public double ActivesCurrentSum
    {
        get => _activesCurrentSum;
        private set => SetProperty(ref _activesCurrentSum, value);
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

    public double ArchivesSoldSum
    {
        get => _archivesSoldSum;
        private set => SetProperty(ref _archivesSoldSum, value);
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

    public double InventorySum
    {
        get => _inventorySum;
        private set => SetProperty(ref _inventorySum, value);
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

    public long Ping
    {
        get => _ping;
        private set => SetProperty(ref _ping, value);
    }

    public PingResult.ServerStatus Status
    {
        get => _status;
        private set => SetProperty(ref _status, value);
    }

    #endregion Properties

    #region Constructor

    public StatisticsModel(ApiClient apiClient, UserModel userModel, IThemeService themeService)
    {
        _apiClient = apiClient;
        _themeService = themeService;

        themeService.ChartThemeChanged += ChartThemeChangedHandler;

        userModel.UserChanged += UserChangedHandler;
        userModel.CurrencyChanged += CurrencyChangedHandler;

        _inventoryGames = Enumerable.Empty<Statistics.InventoryGameStatisticResponse>();
        _inventoryGamesSeries = Enumerable.Empty<ISeries>();

        RefreshPing();
    }

    #endregion Constructor

    #region Methods

    private void ChartThemeChangedHandler(object? sender, ChartThemeChangedEventArgs args)
    {
        GetInventoryGamesSeries();
        GetInvestedSumGrowthSeries();
    }

    private void UserChangedHandler(object? sender)
    {
        RefreshStatistics();
        RefreshPing();
    }

    private void CurrencyChangedHandler(object? sender)
    {
        RefreshStatistics();
    }

    private void GetInvestedSumGrowthSeries()
    {
        double growth = InvestedSumGrowth < 0 ? 0 : InvestedSumGrowth > 1 ? 100 : InvestedSumGrowth * 100;

        InvestedSumGrowthSeries = GaugeGenerator.BuildSolidGauge(
            new GaugeItem(
                90,
                series =>
                {
                    series.AnimationsSpeed = TimeSpan.FromMilliseconds(200);
                    series.MaxRadialColumnWidth = 20;
                    series.Fill = new SolidColorPaint(_themeService.CurrentChartThemeVariant.Colors.ElementAt(1));
                    series.DataLabelsSize = 0;
                }),
            new GaugeItem(GaugeItem.Background, series =>
            {
                series.MaxRadialColumnWidth = 20;
                series.Fill = new SolidColorPaint(_themeService.CurrentChartThemeVariant.Colors.ElementAt(2));
            }));
    }

    private void GetInventoryGamesSeries()
    {
        if(!InventoryGames.Any()) return;
        
        int i = 0;
        InventoryGamesSeries = InventoryGames.OrderByDescending(x => x.Count).AsPieSeries((value, builder) =>
        {
            builder.MaxRadialColumnWidth = 20;
            builder.HoverPushout = 0;
            builder.Mapping = (game, point) => new(point, game.Count);
            builder.ToolTipLabelFormatter = _ => $"{value.GameTitle}: {value.Count}";
            builder.Fill = new SolidColorPaint(_themeService.CurrentChartThemeVariant.Colors.ElementAt(i));
            i++;
        });
    }

    private async void RefreshStatistics()
    {
        Statistics.InvestmentSumResponse? investmentSumResponse =
            await _apiClient.GetAsync<Statistics.InvestmentSumResponse>(ApiConstants.ApiControllers.Statistics,
                "GetInvestmentSum");

        InvestedSum = investmentSumResponse?.TotalSum ?? 0;
        InvestedSumGrowth = investmentSumResponse?.PercentageGrowth ?? 0;


        Statistics.FinancialGoalResponse? financialGoalResponse =
            await _apiClient.GetAsync<Statistics.FinancialGoalResponse>(ApiConstants.ApiControllers.Statistics,
                "GetFinancialGoal");

        FinancialGoal = financialGoalResponse?.FinancialGoal ?? 0;
        FinancialGoalPercentageCompletion = financialGoalResponse?.PercentageCompletion ?? 0;


        Statistics.ItemsCountResponse? itemsCountResponse =
            await _apiClient.GetAsync<Statistics.ItemsCountResponse>(ApiConstants.ApiControllers.Statistics,
                "GetItemsCount");

        TotalCount = itemsCountResponse?.Count ?? 0;


        Statistics.ActiveStatisticResponse? activeStatisticResponse =
            await _apiClient.GetAsync<Statistics.ActiveStatisticResponse>(ApiConstants.ApiControllers.Statistics,
                "GetActiveStatistic");

        ActivesCount = activeStatisticResponse?.Count ?? 0;
        ActivesCurrentSum = activeStatisticResponse?.CurrentSum ?? 0;
        ActivesPercentageGrowth = activeStatisticResponse?.PercentageGrowth ?? 0;


        Statistics.ArchiveStatisticResponse? archiveStatisticResponse =
            await _apiClient.GetAsync<Statistics.ArchiveStatisticResponse>(ApiConstants.ApiControllers.Statistics,
                "GetArchiveStatistic");

        ArchivesCount = archiveStatisticResponse?.Count ?? 0;
        ArchivesSoldSum = archiveStatisticResponse?.SoldSum ?? 0;
        ArchivesPercentageGrowth = archiveStatisticResponse?.PercentageGrowth ?? 0;


        Statistics.InventoryStatisticResponse? inventoryStatisticResponse =
            await _apiClient.GetAsync<Statistics.InventoryStatisticResponse>(ApiConstants.ApiControllers.Statistics,
                "GetInventoryStatistic");

        InventoryCount = inventoryStatisticResponse?.Count ?? 0;
        InventorySum = inventoryStatisticResponse?.Sum ?? 0;
        InventoryGames = inventoryStatisticResponse?.Games ??
                         Enumerable.Empty<Statistics.InventoryGameStatisticResponse>();
    }

    private async void RefreshPing()
    {
        PingResult pingResult = await _apiClient.GetApiPing();
        Ping = pingResult.Ping;
        Status = pingResult.Status;
    }

    #endregion Methods
}
