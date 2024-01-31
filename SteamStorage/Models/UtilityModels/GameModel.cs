namespace SteamStorage.Models.UtilityModels;

public class GameModel
{
    #region Properties

    public int Id { get; }
    public string ImageUrl { get; }
    public string Title { get; }

    #endregion Properties

    #region Constructor

    public GameModel(int id, string imageUrl, string title)
    {
        Id = id;
        ImageUrl = imageUrl;
        Title = title;
    }

    #endregion Constructor
}