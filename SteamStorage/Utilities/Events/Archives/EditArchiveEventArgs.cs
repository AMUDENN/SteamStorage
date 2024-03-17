using SteamStorage.Models.UtilityModels;

namespace SteamStorage.Utilities.Events.Archives;

public class EditArchiveEventArgs
{
    #region Properties

    public ArchiveModel? ArchiveModel { get; }

    #endregion Properties

    #region Constructor

    public EditArchiveEventArgs(
        ArchiveModel? archiveModel)
    {
        ArchiveModel = archiveModel;
    }

    #endregion Constructor
}
