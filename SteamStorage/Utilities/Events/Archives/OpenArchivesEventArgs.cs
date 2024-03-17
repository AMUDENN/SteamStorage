using SteamStorage.Models.UtilityModels;

namespace SteamStorage.Utilities.Events.Archives;

public class OpenArchivesEventArgs
{
    #region Properties

    public ArchiveGroupModel? Group { get; }

    #endregion Properties

    #region Constructor

    public OpenArchivesEventArgs(
        ArchiveGroupModel? group)
    {
        Group = group;
    }

    #endregion Constructor
}
