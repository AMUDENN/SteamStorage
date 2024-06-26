﻿using System;
using System.Collections.Generic;
using System.Linq;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using SteamStorage.Models.Tools.UtilityModels.BaseModels;
using SteamStorage.Services.ThemeService;
using SteamStorage.Utilities;
using SteamStorage.Utilities.Events.Settings;
using SteamStorage.Utilities.ThemeVariants;
using SteamStorageAPI.SDK;
using SteamStorageAPI.SDK.ApiEntities;
using SteamStorageAPI.SDK.Utilities;

namespace SteamStorage.Models.Tools.UtilityModels;

public class ActiveGroupModel : ExtendedBaseGroupModel
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
    private IEnumerable<ActiveGroups.ActiveGroupDynamicResponse>? _groupDynamic;
    private IEnumerable<ISeries> _changeSeries;
    private IEnumerable<Axis> _xAxis;
    private IEnumerable<Axis> _yAxis;

    private PeriodModel? _selectedPeriodModel;

    private bool _isLoading;

    #endregion Fields

    #region Properties
    
    public double? GoalSumCompletion { get; }

    public string BuySumString { get; }

    public string CurrentSumString { get; }

    public decimal? GoalSum { get; }
    
    public string GoalSumString { get; }

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

    private IEnumerable<ActiveGroups.ActiveGroupDynamicResponse>? GroupDynamic
    {
        get => _groupDynamic;
        set
        {
            SetProperty(ref _groupDynamic, value);
            OnPropertyChanged(nameof(NotFoundText));
            GetDynamicChart();
        }
    }

    public string? NotFoundText
    {
        get => GroupDynamic?.Count() == 0 && !IsLoading ? EMPTY_DYNAMIC_TEXT : null;
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

    public ActiveGroupModel(
        ApiClient apiClient,
        PeriodsModel periodsModel,
        IThemeService themeService,
        int groupId,
        string title,
        string colour,
        int count,
        decimal? goalSum,
        double? goalSumCompletion,
        decimal buySum,
        decimal currentSum,
        string currencyMark,
        double change,
        DateTime dateCreation,
        string? description) : base(groupId, title, colour, count, dateCreation, description)
    {
        _apiClient = apiClient;
        _periodsModel = periodsModel;
        _themeService = themeService;
        
        periodsModel.PropertyChanged += (_, e) => OnPropertyChanged(e.PropertyName);
        themeService.ChartThemeChanged += ChartThemeChangedHandler;
        
        GoalSum = goalSum;
        
        GoalSumCompletion = goalSumCompletion;
        
        BuySumString = $"{buySum:N2} {currencyMark}";
        CurrentSumString = $"{currentSum:N2} {currencyMark}";
        GoalSumString = goalSum is null ? "(не установлена)" : $"{goalSum:N2} {currencyMark} ({goalSumCompletion:N0}%)";

        Change = change;

        _changeSeries = Enumerable.Empty<ISeries>();
        _xAxis = Enumerable.Empty<Axis>();
        _yAxis = Enumerable.Empty<Axis>();

        IsLoading = false;
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
            new LineSeries<ActiveGroups.ActiveGroupDynamicResponse>
            {
                Values = GroupDynamic,
                Mapping = (dynamic, point) => new(point, Convert.ToDouble(dynamic.Sum)),
                YToolTipLabelFormatter = index =>
                    $"{index.Model?.DateUpdate.ToString(ProgramConstants.VIEW_DATE_FORMAT)}: {index.Model?.Sum:N2}",
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
                MinLimit = GroupDynamic?.Any() == true
                    ? Convert.ToDouble(GroupDynamic?.Min(x => x.Sum)) / 1.1 - 0.01
                    : 0,
                MaxLimit = GroupDynamic?.Any() == true
                    ? Convert.ToDouble(GroupDynamic?.Max(x => x.Sum)) * 1.1 + 0.01
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

        ActiveGroups.ActiveGroupDynamicStatsResponse? activeGroupDynamicResponse =
            await _apiClient
                .GetAsync<ActiveGroups.ActiveGroupDynamicStatsResponse, ActiveGroups.GetActiveGroupDynamicRequest>(
                    ApiConstants.ApiMethods.GetActiveGroupDynamics,
                    new(GroupId, dateStart, dateEnd));

        ChangePeriod = activeGroupDynamicResponse?.ChangePeriod;

        GroupDynamic = activeGroupDynamicResponse?.Dynamic;

        IsLoading = false;
    }

    #endregion Methods
}
