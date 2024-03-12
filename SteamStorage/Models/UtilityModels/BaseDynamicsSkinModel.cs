using System;
using System.Collections.Generic;
using System.Linq;
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

public class BaseDynamicsSkinModel : BaseSkinModel
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

    #region Constructor

    public BaseDynamicsSkinModel(
        ApiClient apiClient,
        IThemeService themeService,
        int skinId,
        string imageUrl,
        string marketUrl,
        string title) : base(skinId, imageUrl, marketUrl, title)
    {
        _apiClient = apiClient;
        _themeService = themeService;

        themeService.ChartThemeChanged += ChartThemeChangedHandler;

        IsLoading = false;

        _changeSeries = Enumerable.Empty<ISeries>();
        _xAxis = Enumerable.Empty<Axis>();
        _yAxis = Enumerable.Empty<Axis>();
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
