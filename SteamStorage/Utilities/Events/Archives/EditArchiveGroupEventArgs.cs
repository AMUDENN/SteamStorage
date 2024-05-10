using System;
using SteamStorage.Models.Tools.UtilityModels;

namespace SteamStorage.Utilities.Events.Archives;

public class EditArchiveGroupEventArgs : EventArgs
{
    #region Properties

    public ArchiveGroupModel? Group { get; }

    #endregion Properties

    #region Constructor

    public EditArchiveGroupEventArgs(
        ArchiveGroupModel? group)
    {
        Group = group;
    }

    #endregion Constructor
}
