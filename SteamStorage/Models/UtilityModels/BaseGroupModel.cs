using System;
using SteamStorage.Models.Tools;
using SteamStorage.Utilities;

namespace SteamStorage.Models.UtilityModels;

public class BaseGroupModel : ModelBase
{
    #region Properties

    protected int GroupId { get; }
    
    public string Colour { get; }

    public string Title { get; }

    public int Count { get; }
    
    public string DateCreationString { get; }

    #endregion Properties

    #region Constructor

    protected BaseGroupModel(
        int groupId,
        string colour,
        string title,
        int count,
        DateTime dateCreation)
    {
        GroupId = groupId;
        Colour = colour;
        Title = title;
        Count = count;
        DateCreationString = dateCreation.ToString(ProgramConstants.VIEW_DATE_FORMAT);
    }

    #endregion Constructor
}
