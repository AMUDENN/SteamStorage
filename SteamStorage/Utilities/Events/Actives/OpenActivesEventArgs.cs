using SteamStorage.Models.Tools.UtilityModels;

namespace SteamStorage.Utilities.Events.Actives;

public class OpenActivesEventArgs
{
    #region Properties

    public ActiveGroupModel? Group { get; }

    #endregion Properties

    #region Constructor

    public OpenActivesEventArgs(
        ActiveGroupModel? group)
    {
        Group = group;
    }

    #endregion Constructor
}
