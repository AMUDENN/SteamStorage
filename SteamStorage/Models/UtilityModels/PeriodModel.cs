namespace SteamStorage.Models.UtilityModels;

public class PeriodModel
{
    #region Properties

    public string FullTitle { get; }

    public string SmallTitle { get; }

    public int Days { get; }

    #endregion Properties

    #region Constructor

    public PeriodModel(
        string fullTitle,
        string smallTitle,
        int days)
    {
        FullTitle = fullTitle;
        SmallTitle = smallTitle;
        Days = days;
    }

    #endregion Constructor
}
