using SteamStorage.Models.UtilityModels;

namespace SteamStorage.Utilities.Events.Actives;

public class SoldActiveEventArgs
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
