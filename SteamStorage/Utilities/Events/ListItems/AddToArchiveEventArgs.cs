using System;
using SteamStorage.Models.Tools.UtilityModels;

namespace SteamStorage.Utilities.Events.ListItems;

public class AddToArchiveEventArgs : EventArgs
{
    #region Properties

    public ListItemModel? ListItem { get; }

    #endregion Properties

    #region Constructor

    public AddToArchiveEventArgs(
        ListItemModel? listItem)
    {
        ListItem = listItem;
    }

    #endregion Constructor
}
