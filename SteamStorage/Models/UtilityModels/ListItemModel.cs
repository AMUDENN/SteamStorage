using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using SteamStorage.Models.Tools;
using SteamStorage.Services.ThemeService;
using SteamStorage.Utilities;
using SteamStorage.Utilities.Events;
using SteamStorage.Utilities.ThemeVariants;
using SteamStorageAPI;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Utilities;

namespace SteamStorage.Models.UtilityModels;

public class ListItemModel : ModelBase
{
    #region Fields

    private readonly ApiClient _apiClient;
    private readonly IThemeService _themeService;

    private bool _isMarked;

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

    private int Id { get; }

    private string MarketUrl { get; }

    public string ImageUrl { get; }

    public string Title { get; }

    public decimal CurrentPrice { get; }

    public string CurrencyMark { get; }

    public double Change7D { get; }

    public double Change30D { get; }

    public bool IsMarked
    {
        get => _isMarked;
        set
        {
            SetProperty(ref _isMarked, value);
            if (value)
                PostIsMarked();
            else
                DeleteMarked();
        }
    }

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
            GetDynamicChart();
        }
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
            GetDynamicStats(DateTime.Now.AddDays(-1), DateTime.Now);
        }
    }

    public bool IsOneWeekChecked
    {
        get => _isOneWeekChecked;
        set
        {
            SetProperty(ref _isOneWeekChecked, value);
            GetDynamicStats(DateTime.Now.AddDays(-7), DateTime.Now);
        }
    }

    public bool IsOneMonthChecked
    {
        get => _isOneMonthChecked;
        set
        {
            SetProperty(ref _isOneMonthChecked, value);
            GetDynamicStats(DateTime.Now.AddDays(-30), DateTime.Now);
        }
    }

    public bool IsOneYearChecked
    {
        get => _isOneYearChecked;
        set
        {
            SetProperty(ref _isOneYearChecked, value);
            GetDynamicStats(DateTime.Now.AddDays(-365), DateTime.Now);
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        private set => SetProperty(ref _isLoading, value);
    }

    #endregion Properties

    #region Commands

    public RelayCommand OpenInSteamCommand { get; }

    public RelayCommand AddToActivesCommand { get; }

    public RelayCommand AddToArchiveCommand { get; }

    #endregion Commands

    #region Constructor

    public ListItemModel(ApiClient apiClient, IThemeService themeService, int id, string imageUrl, string marketUrl,
        string title,
        decimal currentPrice, string currencyMark, double change7D, double change30D, bool isMarked)
    {
        _apiClient = apiClient;
        _themeService = themeService;

        themeService.ChartThemeChanged += ChartThemeChangedHandler;

        Id = id;
        ImageUrl = imageUrl;
        MarketUrl = marketUrl;
        Title = title;
        CurrentPrice = currentPrice;
        CurrencyMark = currencyMark;
        Change7D = change7D;
        Change30D = change30D;
        _isMarked = isMarked;

        IsLoading = false;

        _changeSeries = Enumerable.Empty<ISeries>();
        _xAxis = Enumerable.Empty<Axis>();
        _yAxis = Enumerable.Empty<Axis>();

        OpenInSteamCommand = new(DoOpenInSteamCommand);
        AddToActivesCommand = new(DoAddToActivesCommand);
        AddToArchiveCommand = new(DoAddToArchiveCommand);
    }

    #endregion Constructor

    #region Methods

    private void ChartThemeChangedHandler(object? sender, ChartThemeChangedEventArgs args)
    {
        GetDynamicChart();
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
                DataLabelsFormatter = index => $"{index.Model?.DateUpdate}: {index.Model?.Price}",
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
                    ? Convert.ToDouble(SkinDynamic?.Min(x => x.Price)) / 1.1
                    : 0,
                MaxLimit = SkinDynamic?.Any() == true
                    ? Convert.ToDouble(SkinDynamic?.Max(x => x.Price)) * 1.1
                    : 0,
                Labels = null,
                LabelsPaint = null,
                SeparatorsPaint = null
            }
        };
    }

    private void DoOpenInSteamCommand()
    {
        UrlUtility.OpenUrl(MarketUrl);
    }

    private void DoAddToActivesCommand()
    {

    }

    private void DoAddToArchiveCommand()
    {

    }

    private async void GetDynamicStats(DateTime dateStart, DateTime dateEnd)
    {
        IsLoading = true;
        DatePeriod =
            $"{dateStart.ToString(ProgramConstants.PERIOD_DATE_FORMAT)} - {dateEnd.ToString(ProgramConstants.PERIOD_DATE_FORMAT)}";

        Skins.SkinDynamicStatsResponse? skinDynamicsResponse =
            await _apiClient.GetAsync<Skins.SkinDynamicStatsResponse, Skins.GetSkinDynamicsRequest>(
                ApiConstants.ApiControllers.Skins,
                "GetSkinDynamics",
                new(Id, dateStart, dateEnd));

        ChangePeriod = skinDynamicsResponse?.ChangePeriod;

        SkinDynamic = skinDynamicsResponse?.Dynamic;

        IsLoading = false;
    }

    private async void PostIsMarked()
    {
        await _apiClient.PostAsync(ApiConstants.ApiControllers.Skins, "SetMarkedSkin",
            new Skins.SetMarkedSkinRequest(Id));
    }

    private async void DeleteMarked()
    {
        await _apiClient.DeleteAsync(ApiConstants.ApiControllers.Skins, "DeleteMarkedSkin",
            new Skins.DeleteMarkedSkinRequest(Id));
    }

    #endregion Methods
}
