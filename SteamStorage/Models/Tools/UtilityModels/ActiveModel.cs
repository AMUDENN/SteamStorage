using System;
using SteamStorage.Models.Tools.UtilityModels.BaseModels;
using SteamStorage.Services.ThemeService;
using SteamStorage.Utilities;
using SteamStorageAPI.SDK.ApiClient;

namespace SteamStorage.Models.Tools.UtilityModels;

public class ActiveModel : BaseDynamicsSkinModel
{
    #region Properties

    public int ActiveId { get; }

    public int GroupId { get; }

    public int Count { get; }

    public decimal BuyPrice { get; }

    public string BuyPriceString { get; }

    public decimal CurrentPrice { get; }

    public string CurrentPriceString { get; }

    public string CurrentSumString { get; }

    public decimal? GoalPrice { get; }

    public string GoalPriceString { get; }

    public decimal? GoalPriceCompletion { get; }

    public decimal Change { get; }

    public DateTime BuyDate { get; }

    public string BuyDateString { get; }

    public string? Description { get; }

    #endregion Properties

    #region Constructor

    public ActiveModel(
        IApiClient apiClient,
        PeriodsModel periodsModel,
        IThemeService themeService,
        int skinId,
        string imageUrl,
        string marketUrl,
        string title,
        int activeId,
        int groupId,
        int count,
        decimal buyPrice,
        decimal currentPrice,
        decimal currentSum,
        decimal? goalPrice,
        decimal? goalPriceCompletion,
        string currencyMark,
        decimal change,
        DateTime buyDate,
        string? description) : base(apiClient, periodsModel, themeService, skinId, imageUrl, marketUrl, title)
    {
        ActiveId = activeId;
        GroupId = groupId;

        Count = count;

        BuyPrice = buyPrice;
        CurrentPrice = currentPrice;
        GoalPrice = goalPrice;
        GoalPriceCompletion = goalPriceCompletion;

        BuyPriceString = $"{buyPrice:N2} {currencyMark}";
        CurrentPriceString = $"{currentPrice:N2} {currencyMark}";
        CurrentSumString = $"{currentSum:N2} {currencyMark}";
        GoalPriceString = goalPrice is null
            ? "(не установлена)"
            : $"{goalPrice:N2} {currencyMark} ({goalPriceCompletion:N0}%)";

        Change = change;

        BuyDate = buyDate;

        BuyDateString = buyDate.ToString(ProgramConstants.VIEW_DATE_FORMAT);

        Description = description;
    }

    #endregion Constructor
}
