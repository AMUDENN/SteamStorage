using SteamStorage.Models.UtilityModels;

namespace SteamStorage.Utilities.Events.ListItems;

public class AddToActivesEventArgs
{
    #region Properties

    public ListItemModel? ListItem { get; }

    #endregion Properties

    #region Constructor

    public AddToActivesEventArgs(
        ListItemModel? listItem)
    {
        ListItem = listItem;
    }

    #endregion Constructor
}
