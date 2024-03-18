using System;

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
        DateTime dateCreation) : base(groupId, colour, title, count, dateCreation)
    {
        BuySumString = $"{buySum:N2} {currencyMark}";
        SoldSumString = $"{soldSum:N2} {currencyMark}";

        Change = change;
    }

    #endregion Constructor
}
