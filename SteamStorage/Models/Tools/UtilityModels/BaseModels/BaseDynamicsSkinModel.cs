﻿using System;
using System.Collections.Generic;
using System.Linq;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using SteamStorage.Services.ThemeService;
using SteamStorage.Utilities;
using SteamStorage.Utilities.Events.Settings;
using SteamStorage.Utilities.ThemeVariants;
using SteamStorageAPI.SDK;
using SteamStorageAPI.SDK.ApiEntities;
using SteamStorageAPI.SDK.Utilities;

namespace SteamStorage.Models.Tools.UtilityModels.BaseModels;

public class BaseDynamicsSkinModel : BaseSkinModel
{
    #region Constants

    private const string EMPTY_DYNAMIC_TEXT = "Динамика цены за данный период не найдена";

    #endregion Constants

    #region Fields

    private readonly ApiClient _apiClient;
    private readonly PeriodsModel _periodsModel;
    private readonly IThemeService _themeService;

    private double? _changePeriod;
    private string? _datePeriod;
    private IEnumerable<Skins.SkinDynamicResponse>? _skinDynamic;
    private IEnumerable<ISeries> _changeSeries;
    private IEnumerable<Axis> _xAxis;
    private IEnumerable<Axis> _yAxis;

    private PeriodModel? _selectedPeriodModel;

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

    public IEnumerable<PeriodModel> PeriodModels
    {
        get => _periodsModel.PeriodModels;
    }
    
    public PeriodModel? SelectedPeriodModel
    {
        get => _selectedPeriodModel;
        set
        {
            SetProperty(ref _selectedPeriodModel, value);
            if (_selectedPeriodModel is not null)
                GetDynamicStatsAsync(DateTime.Now.AddDays(-_selectedPeriodModel.Days), DateTime.Now);
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

    protected BaseDynamicsSkinModel(
        ApiClient apiClient,
        PeriodsModel periodsModel,
        IThemeService themeService,
        int skinId,
        string imageUrl,
        string marketUrl,
        string title) : base(skinId, imageUrl, marketUrl, title)
    {
        _apiClient = apiClient;
        _periodsModel = periodsModel;
        _themeService = themeService;
        
        periodsModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
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
        SelectedPeriodModel = _periodsModel.GetDefault();
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
                    $"{index.Model?.DateUpdate.ToString(ProgramConstants.VIEW_DATE_FORMAT)}: {index.Model?.Price:N2}",
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

    private async void GetDynamicStatsAsync(DateTime dateStart, DateTime dateEnd)
    {
        IsLoading = true;
        DatePeriod =
            $"{dateStart.ToString(ProgramConstants.VIEW_DATE_FORMAT)} - {dateEnd.ToString(ProgramConstants.VIEW_DATE_FORMAT)}";

        Skins.SkinDynamicStatsResponse? skinDynamicsResponse =
            await _apiClient.GetAsync<Skins.SkinDynamicStatsResponse, Skins.GetSkinDynamicsRequest>(
                ApiConstants.ApiMethods.GetSkinDynamics,
                new(SkinId, dateStart, dateEnd));

        ChangePeriod = skinDynamicsResponse?.ChangePeriod;

        SkinDynamic = skinDynamicsResponse?.Dynamic;

        IsLoading = false;
    }

    #endregion Methods
}
