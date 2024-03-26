using System;
using SteamStorage.Models.UtilityModels;

namespace SteamStorage.Utilities.Events.Edit;

public class GoingBackEventArgs : EventArgs
{
    #region Properties

    public NavigationModel NavigationModel { get; }

    public SecondaryNavigationModel SecondaryNavigationModel { get; }

    #endregion Properties

    #region Constructor

    public GoingBackEventArgs(
        NavigationModel navigationModel,
        SecondaryNavigationModel secondaryNavigationModel)
    {
        NavigationModel = navigationModel;
        SecondaryNavigationModel = secondaryNavigationModel;
    }

    #endregion Constructor
}
