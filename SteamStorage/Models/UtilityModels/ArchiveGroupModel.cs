using System;
using SteamStorage.Models.UtilityModels.BaseModels;

namespace SteamStorage.Models.UtilityModels;

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
        string colour,
        string title,
        int count,
        decimal buySum,
        decimal soldSum,
        string currencyMark,
        double change,
        DateTime dateCreation,
        string? description) : base(groupId, colour, title, count, dateCreation, description)
    {
        BuySumString = $"{buySum:N2} {currencyMark}";
        SoldSumString = $"{soldSum:N2} {currencyMark}";

        Change = change;
    }

    #endregion Constructor
}
