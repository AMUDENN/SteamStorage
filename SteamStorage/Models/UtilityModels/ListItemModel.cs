using System;
using System.Threading.Tasks;
using SteamStorage.Models.Tools;
using SteamStorage.Utilities;
using SteamStorageAPI;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Utilities;

namespace SteamStorage.Models.UtilityModels;

public class ListItemModel : ModelBase
{
    #region Fields

    private readonly ApiClient _apiClient;

    private bool _isMarked;

    private double? _changePeriod;
    private string? _datePeriod;

    private bool _isOneDayChecked;
    private bool _isOneWeekChecked;
    private bool _isOneMonthChecked;
    private bool _isOneYearChecked;

    private bool _isLoading;

    #endregion Fields

    #region Properties

    public int Id { get; }
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

    #region Constructor

    public ListItemModel(ApiClient apiClient, int id, string imageUrl, string title, decimal currentPrice,
        string currencyMark, double change7D, double change30D, bool isMarked)
    {
        _apiClient = apiClient;
        
        Id = id;
        ImageUrl = imageUrl;
        Title = title;
        CurrentPrice = currentPrice;
        CurrencyMark = currencyMark;
        Change7D = change7D;
        Change30D = change30D;
        _isMarked = isMarked;

        IsLoading = false;
    }

    #endregion Constructor

    #region Methods

    public void UpdateStats()
    {
        if (!(IsOneDayChecked || IsOneWeekChecked || IsOneYearChecked))
            IsOneMonthChecked = true;
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