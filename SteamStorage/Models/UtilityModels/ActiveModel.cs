﻿using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using SteamStorage.Services.ThemeService;
using SteamStorage.Utilities;
using SteamStorage.Utilities.Events;
using SteamStorage.Utilities.ThemeVariants;
using SteamStorageAPI;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Utilities;

namespace SteamStorage.Models.UtilityModels;

public class ActiveModel : BaseSkinModel
{
    #region Constants

    private const string EMPTY_DYNAMIC_TEXT = "Динамика цены за данный период не найдена";

    #endregion Constants

    #region Fields

    private readonly ApiClient _apiClient;
    private readonly IThemeService _themeService;

    private double? _changePeriod;
    private string? _datePeriod;
    private IEnumerable<Skins.SkinDynamicResponse>? _skinDynamic;
    private IEnumerable<ISeries> _changeSeries;
    private IEnumerable<Axis> _xAxis;
    private IEnumerable<Axis> _yAxis;

    private bool _isOneDayChecked;
    private bool _isOneWeekChecked;
    private bool _isOneMonthChecked;
    private bool _isOneYearChecked;

    private bool _isLoading;

    #endregion Fields

    #region Properties

    public int Count { get; }
    
    public string BuyPriceString { get; }
    
    public string CurrentPriceString { get; }
    
    public string CurrentSumString { get; }
    
    public string GoalPriceString { get; }
    
    public string BuyDateString { get; }

    public double Change { get; }

    public double? ChangePeriod
    {
        get => _changePeriod;
        private set => SetProperty(ref _changePeriod, value);
    }

    public string? DatePeriod
    {
        get => _datePeriod;
        private set => SetProperty(ref _datePeriod, value);
    }

    private IEnumerable<Skins.SkinDynamicResponse>? SkinDynamic
    {
        get => _skinDynamic;
        set
        {
            SetProperty(ref _skinDynamic, value);
            OnPropertyChanged(nameof(NotFoundText));
            GetDynamicChart();
        }
    }

    public string? NotFoundText
    {
        get => SkinDynamic?.Count() == 0 && !IsLoading ? EMPTY_DYNAMIC_TEXT : null;
    }

    public IEnumerable<ISeries> ChangeSeries
    {
        get => _changeSeries;
        private set => SetProperty(ref _changeSeries, value);
    }

    public IEnumerable<Axis> XAxis
    {
        get => _xAxis;
        private set => SetProperty(ref _xAxis, value);
    }

    public IEnumerable<Axis> YAxis
    {
        get => _yAxis;
        private set => SetProperty(ref _yAxis, value);
    }

    public bool IsOneDayChecked
    {
        get => _isOneDayChecked;
        set
        {
            SetProperty(ref _isOneDayChecked, value);
            if (value) GetDynamicStats(DateTime.Now.AddDays(-1), DateTime.Now);
        }
    }

    public bool IsOneWeekChecked
    {
        get => _isOneWeekChecked;
        set
        {
            SetProperty(ref _isOneWeekChecked, value);
            if (value) GetDynamicStats(DateTime.Now.AddDays(-7), DateTime.Now);
        }
    }

    public bool IsOneMonthChecked
    {
        get => _isOneMonthChecked;
        set
        {
            SetProperty(ref _isOneMonthChecked, value);
            if (value) GetDynamicStats(DateTime.Now.AddDays(-30), DateTime.Now);
        }
    }

    public bool IsOneYearChecked
    {
        get => _isOneYearChecked;
        set
        {
            SetProperty(ref _isOneYearChecked, value);
            if (value) GetDynamicStats(DateTime.Now.AddDays(-365), DateTime.Now);
        }
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

    #endregion Properties

    #region Commands

    public RelayCommand EditCommand { get; }

    public RelayCommand DeleteCommand { get; }

    #endregion Commands

    #region Constructor

    public ActiveModel(
        ApiClient apiClient,
        IThemeService themeService,
        int skinId,
        string imageUrl,
        string marketUrl,
        string title,
        int count,
        decimal buyPrice,
        decimal currentPrice,
        decimal currentSum,
        decimal? goalPrice,
        double? goalPriceCompletion,
        string currencyMark,
        double change,
        DateTime buyDate) : base(skinId, imageUrl, marketUrl, title)
    {
        _apiClient = apiClient;
        _themeService = themeService;

        themeService.ChartThemeChanged += ChartThemeChangedHandler;

        Count = count;

        BuyPriceString = $"{buyPrice:N2} {currencyMark}";
        CurrentPriceString = $"{currentPrice:N2} {currencyMark}";
        CurrentSumString = $"{currentSum:N2} {currencyMark}";
        GoalPriceString = goalPrice is null
            ? "(не установлена)"
            : $"{goalPrice:N2} {currencyMark} ({goalPriceCompletion:N0}%)";

        BuyDateString = buyDate.ToString(ProgramConstants.VIEW_DATE_FORMAT);

        Change = change;

        IsLoading = false;

        _changeSeries = Enumerable.Empty<ISeries>();
        _xAxis = Enumerable.Empty<Axis>();
        _yAxis = Enumerable.Empty<Axis>();

        EditCommand = new(DoEditCommand);
        DeleteCommand = new(DoDeleteCommand);
    }

    #endregion Constructor

    #region Methods

    private void ChartThemeChangedHandler(object? sender, ChartThemeChangedEventArgs args)
    {
        GetDynamicChart();
    }

    private void DoEditCommand()
    {

    }

    private void DoDeleteCommand()
    {

    }

    public void UpdateStats()
    {
        if (!(IsOneDayChecked || IsOneWeekChecked || IsOneYearChecked))
            IsOneMonthChecked = true;
    }

    private void GetDynamicChart()
    {
        SKColor chartColor = ChangePeriod < 0
            ? _themeService.CurrentChartThemeVariant.GetChartColor(ChartThemeVariants.ChartColors.Negative).Color
            : _themeService.CurrentChartThemeVariant.GetChartColor(ChartThemeVariants.ChartColors.Positive).Color;

        ChangeSeries = new[]
        {
            new LineSeries<Skins.SkinDynamicResponse>
            {
                Values = SkinDynamic,
                Mapping = (dynamic, point) => new(point, Convert.ToDouble(dynamic.Price)),
                YToolTipLabelFormatter = index =>
                    $"{index.Model?.DateUpdate.ToString(ProgramConstants.VIEW_DATE_FORMAT)}: {index.Model?.Price}",
                Stroke = new SolidColorPaint(chartColor) { StrokeThickness = 2 },
                Fill = null,
                LineSmoothness = 0,
                GeometryFill = new SolidColorPaint(chartColor),
                GeometrySize = 6,
                GeometryStroke = null
            }
        };

        XAxis = new[]
        {
            new Axis
            {
                Labels = null,
                LabelsPaint = null,
                SeparatorsPaint = null
            }
        };

        YAxis = new[]
        {
            new Axis
            {
                MinLimit = SkinDynamic?.Any() == true
                    ? Convert.ToDouble(SkinDynamic?.Min(x => x.Price)) / 1.1 - 0.01
                    : 0,
                MaxLimit = SkinDynamic?.Any() == true
                    ? Convert.ToDouble(SkinDynamic?.Max(x => x.Price)) * 1.1 + 0.01
                    : 0,
                Labels = null,
                LabelsPaint = null,
                SeparatorsPaint = null
            }
        };
    }

    private async void GetDynamicStats(DateTime dateStart, DateTime dateEnd)
    {
        IsLoading = true;
        DatePeriod =
            $"{dateStart.ToString(ProgramConstants.VIEW_DATE_FORMAT)} - {dateEnd.ToString(ProgramConstants.VIEW_DATE_FORMAT)}";

        Skins.SkinDynamicStatsResponse? skinDynamicsResponse =
            await _apiClient.GetAsync<Skins.SkinDynamicStatsResponse, Skins.GetSkinDynamicsRequest>(
                ApiConstants.ApiControllers.Skins,
                "GetSkinDynamics",
                new(SkinId, dateStart, dateEnd));

        ChangePeriod = skinDynamicsResponse?.ChangePeriod;

        SkinDynamic = skinDynamicsResponse?.Dynamic;

        IsLoading = false;
    }

    #endregion Methods
}
