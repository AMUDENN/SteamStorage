using System;
using SteamStorage.Utilities;

namespace SteamStorage.Models.Tools.UtilityModels.BaseModels;

public class ExtendedBaseGroupModel : BaseGroupModel
{
    #region Properties

    public int Count { get; }
    
    public DateTime DateCreation { get; }

    public string DateCreationString { get; }
    
    public string? Description { get; }

    #endregion Properties

    #region Constructor

    protected ExtendedBaseGroupModel(
        int groupId,
        string title,
        string colour,
        int count,
        DateTime dateCreation,
        string? description) : base(groupId, title, colour)
    {
        Count = count;
        DateCreation = dateCreation;
        DateCreationString = dateCreation.ToString(ProgramConstants.VIEW_DATE_FORMAT);
        Description = description;
    }

    #endregion Constructor
}
