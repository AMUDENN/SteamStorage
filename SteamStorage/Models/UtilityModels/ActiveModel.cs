using System;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Services.ThemeService;
using SteamStorage.Utilities;
using SteamStorageAPI;

namespace SteamStorage.Models.UtilityModels;

public class ActiveModel : BaseDynamicsSkinModel
{
    #region Properties

    public int Count { get; }

    public string BuyPriceString { get; }

    public string CurrentPriceString { get; }

    public string CurrentSumString { get; }

    public string GoalPriceString { get; }

    public double Change { get; }

    public string BuyDateString { get; }

    #endregion Properties

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
        DateTime buyDate) : base(apiClient, themeService, skinId, imageUrl, marketUrl, title)
    {
        Count = count;

        BuyPriceString = $"{buyPrice:N2} {currencyMark}";
        CurrentPriceString = $"{currentPrice:N2} {currencyMark}";
        CurrentSumString = $"{currentSum:N2} {currencyMark}";
        GoalPriceString = goalPrice is null
            ? "(не установлена)"
            : $"{goalPrice:N2} {currencyMark} ({goalPriceCompletion:N0}%)";

        Change = change;

        BuyDateString = buyDate.ToString(ProgramConstants.VIEW_DATE_FORMAT);
    }

    #endregion Constructor
}
