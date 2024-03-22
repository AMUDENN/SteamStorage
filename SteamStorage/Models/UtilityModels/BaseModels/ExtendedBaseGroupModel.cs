using System;
using SteamStorage.Utilities;

namespace SteamStorage.Models.UtilityModels.BaseModels;

public class ExtendedBaseGroupModel : BaseGroupModel
{
    #region Properties

    public string Colour { get; }

    public int Count { get; }

    public string DateCreationString { get; }

    #endregion Properties

    #region Constructor

    protected ExtendedBaseGroupModel(
        int groupId,
        string colour,
        string title,
        int count,
        DateTime dateCreation) : base(groupId, title)
    {
        Colour = colour;
        Count = count;
        DateCreationString = dateCreation.ToString(ProgramConstants.VIEW_DATE_FORMAT);
    }

    #endregion Constructor
}
