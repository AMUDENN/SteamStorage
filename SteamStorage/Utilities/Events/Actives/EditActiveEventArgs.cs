using System;
using SteamStorage.Models.UtilityModels;

namespace SteamStorage.Utilities.Events.Actives;

public class EditActiveEventArgs : EventArgs
{
    #region Properties

    public ActiveModel? ActiveModel { get; }

    #endregion Properties

    #region Constructor

    public EditActiveEventArgs(
        ActiveModel? activeModel)
    {
        ActiveModel = activeModel;
    }

    #endregion Constructor
}
