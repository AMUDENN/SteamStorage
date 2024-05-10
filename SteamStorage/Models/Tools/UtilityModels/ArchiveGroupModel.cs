using System;
using SteamStorage.Models.Tools.UtilityModels.BaseModels;

namespace SteamStorage.Models.Tools.UtilityModels;

public class ArchiveGroupModel : ExtendedBaseGroupModel
{
    #region Properties

    public string BuySumString { get; }

    public string SoldSumString { get; }

    public double Change { get; }

    #endregion Properties

    #region Constructor

    public ArchiveGroupModel(
        int groupId,
        string title,
        string colour,
        int count,
        decimal buySum,
        decimal soldSum,
        string currencyMark,
        double change,
        DateTime dateCreation,
        string? description) : base(groupId, title, colour, count, dateCreation, description)
    {
        BuySumString = $"{buySum:N2} {currencyMark}";
        SoldSumString = $"{soldSum:N2} {currencyMark}";

        Change = change;
    }

    #endregion Constructor
}
