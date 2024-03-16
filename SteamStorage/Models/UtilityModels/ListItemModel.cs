using CommunityToolkit.Mvvm.Input;
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

    #region Commands

    public RelayCommand AddToActivesCommand { get; }

    public RelayCommand AddToArchiveCommand { get; }

    #endregion Commands

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

        CurrentPriceString = $"{currentPrice:N2} {currencyMark}";

        Change7D = change7D;
        Change30D = change30D;

        _isMarked = isMarked;

        AddToActivesCommand = new(DoAddToActivesCommand);
        AddToArchiveCommand = new(DoAddToArchiveCommand);
    }

    #endregion Constructor

    #region Methods

    private void DoAddToActivesCommand()
    {

    }

    private void DoAddToArchiveCommand()
    {

    }

    private async void PostIsMarked()
    {
        await _apiClient.PostAsync(
            ApiConstants.ApiControllers.Skins, 
            ApiConstants.ApiMethods.SetMarkedSkin,
            new Skins.SetMarkedSkinRequest(SkinId));
    }

    private async void DeleteMarked()
    {
        await _apiClient.DeleteAsync(
            ApiConstants.ApiControllers.Skins, 
            ApiConstants.ApiMethods.DeleteMarkedSkin,
            new Skins.DeleteMarkedSkinRequest(SkinId));
    }

    #endregion Methods
}
