using System;
using SteamStorage.Models.UtilityModels;

namespace SteamStorage.Utilities.Events.Actives;

public class EditActiveGroupEventArgs : EventArgs
{
    #region Properties

    public ActiveGroupModel? Group { get; }

    #endregion Properties

    #region Constructor

    public EditActiveGroupEventArgs(
        ActiveGroupModel? group)
    {
        Group = group;
    }

    #endregion Constructor
}
