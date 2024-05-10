using System;
using SteamStorage.Models.Tools.UtilityModels;

namespace SteamStorage.Utilities.Events.Archives;

public class EditArchiveEventArgs : EventArgs
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
