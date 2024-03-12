using SteamStorage.Services.ThemeService;
using SteamStorageAPI;

namespace SteamStorage.Models.UtilityModels;

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
        IThemeService themeService,
        int skinId,
        string imageUrl,
        string marketUrl,
        string title,
        int count,
        decimal currentPrice,
        decimal currentSum,
        string currencyMark) : base(apiClient, themeService, skinId, imageUrl, marketUrl, title)
    {
        Count = count;

        CurrentPriceString = $"{currentPrice:N2} {currencyMark}";
        CurrentSumString = $"{currentSum:N2} {currencyMark}";
    }

    #endregion Constructor
}
