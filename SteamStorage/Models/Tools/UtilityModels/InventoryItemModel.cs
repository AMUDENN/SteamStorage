using SteamStorage.Models.Tools.UtilityModels.BaseModels;
using SteamStorage.Services.ThemeService;
using SteamStorageAPI.SDK;

namespace SteamStorage.Models.Tools.UtilityModels;

public class InventoryItemModel : BaseDynamicsSkinModel
{
    #region Properties

    public int Count { get; }

    public string CurrentPriceString { get; }

    public string CurrentSumString { get; }

    #endregion Properties

    #region Constructor

    public InventoryItemModel(
        ApiClient apiClient,
        PeriodsModel periodsModel,
        IThemeService themeService,
        int skinId,
        string imageUrl,
        string marketUrl,
        string title,
        int count,
        decimal currentPrice,
        decimal currentSum,
        string currencyMark) : base(apiClient, periodsModel, themeService, skinId, imageUrl, marketUrl, title)
    {
        Count = count;

        CurrentPriceString = $"{currentPrice:N2} {currencyMark}";
        CurrentSumString = $"{currentSum:N2} {currencyMark}";
    }

    #endregion Constructor
}
