using SteamStorage.Models.Tools;

namespace SteamStorage.Models.UtilityModels.BaseModels;

public class BaseGroupModel : ModelBase
{
    #region Properties

    public int GroupId { get; }

    public string Title { get; }

    #endregion Properties

    #region Constructor

    public BaseGroupModel(
        int groupId,
        string title)
    {
        GroupId = groupId;
        Title = title;
    }

    #endregion Constructor
}
