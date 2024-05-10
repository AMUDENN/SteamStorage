using System;
using SteamStorage.Models.Tools.UtilityModels;

namespace SteamStorage.Utilities.Events.Actives;

public class SoldActiveEventArgs : EventArgs
{
    #region Properties

    public ActiveModel? ActiveModel { get; }

    #endregion Properties

    #region Constructor

    public SoldActiveEventArgs(
        ActiveModel? activeModel)
    {
        ActiveModel = activeModel;
    }

    #endregion Constructor
}
