using System;
using SteamStorage.Models.UtilityModels;

namespace SteamStorage.Utilities.Events.Actives;

public class AddActiveEventArgs : EventArgs
{
    #region Properties

    public ActiveGroupModel? Group { get; }

    #endregion Properties

    #region Constructor

    public AddActiveEventArgs(
        ActiveGroupModel? group)
    {
        Group = group;
    }

    #endregion Constructor
}
