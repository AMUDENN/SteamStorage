namespace SteamStorage.Models.Tools.UtilityModels.BaseModels;

public class BaseGroupModel : ModelBase
{
    #region Properties

    public int GroupId { get; }

    public string Title { get; }
    
    public string Colour { get; }

    #endregion Properties

    #region Constructor

    public BaseGroupModel(
        int groupId,
        string title,
        string colour)
    {
        GroupId = groupId;
        Title = title;
        Colour = colour;
    }

    #endregion Constructor
}
