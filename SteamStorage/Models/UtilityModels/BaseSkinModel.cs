using CommunityToolkit.Mvvm.Input;
using SteamStorage.Models.Tools;
using SteamStorageAPI.Utilities;

namespace SteamStorage.Models.UtilityModels;

public class BaseSkinModel : ModelBase
{
    #region Properties

    protected int SkinId { get; }

    private string MarketUrl { get; }

    public string ImageUrl { get; }

    public string Title { get; }

    #endregion Properties

    #region Commands

    public RelayCommand OpenInSteamCommand { get; }

    #endregion Commands

    #region Constructor

    public BaseSkinModel(int skinId, string imageUrl, string marketUrl, string title)
    {
        SkinId = skinId;
        ImageUrl = imageUrl;
        MarketUrl = marketUrl;
        Title = title;

        OpenInSteamCommand = new(DoOpenInSteamCommand);
    }

    #endregion Constructor

    #region Methods

    private void DoOpenInSteamCommand()
    {
        UrlUtility.OpenUrl(MarketUrl);
    }

    #endregion Methods
}