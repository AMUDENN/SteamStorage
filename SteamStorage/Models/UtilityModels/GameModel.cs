namespace SteamStorage.Models.UtilityModels;

public class GameModel
{
    #region Properties

    public string ImageUrl { get; }
    public string Title { get; }

    #endregion Properties

    #region Constructor

    public GameModel(string imageUrl, string title)
    {
        ImageUrl = imageUrl;
        Title = title;
    }

    #endregion Constructor
}