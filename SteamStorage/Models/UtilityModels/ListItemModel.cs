using SteamStorage.Models.UtilityModels.BaseModels;
using SteamStorage.Services.ThemeService;
using SteamStorageAPI;
using SteamStorageAPI.ApiEntities;
using SteamStorageAPI.Utilities;

namespace SteamStorage.Models.UtilityModels;

public class ListItemModel : BaseDynamicsSkinModel
{
    #region Fields

    private readonly ApiClient _apiClient;

    private bool _isMarked;

    #endregion Fields

    #region Properties

    public decimal CurrentPrice { get; }

    public string CurrentPriceString { get; }

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

    #endregion Properties

    #region Constructor

    public ListItemModel(
        ApiClient apiClient,
        IThemeService themeService,
        int skinId,
        string imageUrl,
        string marketUrl,
        string title,
        decimal currentPrice,
        string currencyMark,
        double change7D,
        double change30D,
        bool isMarked) : base(apiClient, themeService, skinId, imageUrl, marketUrl, title)
    {
        _apiClient = apiClient;

        CurrentPrice = currentPrice;

        CurrentPriceString = $"{currentPrice:N2} {currencyMark}";

        Change7D = change7D;
        Change30D = change30D;

        _isMarked = isMarked;
    }

    #endregion Constructor

    #region Methods

    private async void PostIsMarked()
    {
        await _apiClient.PostAsync(
            ApiConstants.ApiMethods.SetMarkedSkin,
            new Skins.SetMarkedSkinRequest(SkinId));
    }

    private async void DeleteMarked()
    {
        await _apiClient.DeleteAsync(
            ApiConstants.ApiMethods.DeleteMarkedSkin,
            new Skins.DeleteMarkedSkinRequest(SkinId));
    }

    #endregion Methods
}
