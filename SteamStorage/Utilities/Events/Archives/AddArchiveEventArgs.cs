using System;
using SteamStorage.Models.Tools.UtilityModels;

namespace SteamStorage.Utilities.Events.Archives;

public class AddArchiveEventArgs : EventArgs
{
    #region Properties

    public ArchiveGroupModel? Group { get; }

    #endregion Properties

    #region Constructor

    public AddArchiveEventArgs(
        ArchiveGroupModel? group)
    {
        Group = group;
    }

    #endregion Constructor
}
