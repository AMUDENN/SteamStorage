using System;
using SteamStorage.Utilities;

namespace SteamStorage.Models.UtilityModels.BaseModels;

public class ExtendedBaseGroupModel : BaseGroupModel
{
    #region Properties

    public string Colour { get; }

    public int Count { get; }
    
    public DateTime DateCreation { get; }

    public string DateCreationString { get; }
    
    public string? Description { get; }

    #endregion Properties

    #region Constructor

    protected ExtendedBaseGroupModel(
        int groupId,
        string colour,
        string title,
        int count,
        DateTime dateCreation,
        string? description) : base(groupId, title)
    {
        Colour = colour;
        Count = count;
        DateCreation = dateCreation;
        DateCreationString = dateCreation.ToString(ProgramConstants.VIEW_DATE_FORMAT);
        Description = description;
    }

    #endregion Constructor
}
