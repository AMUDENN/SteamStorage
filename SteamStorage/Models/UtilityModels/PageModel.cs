namespace SteamStorage.Models.UtilityModels;

public class PageModel
{
    #region Properties

    public int Id { get; }

    public string Title { get; }

    #endregion Properties

    #region Constructor

    public PageModel(
        int id,
        string title)
    {
        Id = id;
        Title = title;
    }

    #endregion Constructor
}
