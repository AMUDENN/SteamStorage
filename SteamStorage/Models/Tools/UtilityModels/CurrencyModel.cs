namespace SteamStorage.Models.Tools.UtilityModels;

public class CurrencyModel
{
    #region Properties

    public int Id { get; }

    public string Title { get; }

    public string Mark { get; }

    #endregion Properties

    #region Constructor

    public CurrencyModel(
        int id,
        string title,
        string mark)
    {
        Id = id;
        Title = title;
        Mark = mark;
    }

    #endregion Constructor
}
