using System;
using SteamStorage.Models.UtilityModels.BaseModels;
using SteamStorage.Utilities;

namespace SteamStorage.Models.UtilityModels;

public class ArchiveModel : BaseSkinModel
{
    #region Properties

    public int ArchiveId { get; }

    public int GroupId { get; }

    public int Count { get; }

    public decimal BuyPrice { get; }

    public string BuyPriceString { get; }

    public decimal SoldPrice { get; }

    public string SoldPriceString { get; }

    public string SoldSumString { get; }

    public double Change { get; }

    public DateTime BuyDate { get; }

    public string BuyDateString { get; }

    public DateTime SoldDate { get; }

    public string SoldDateString { get; }

    #endregion Properties

    #region Constructor

    public ArchiveModel(
        int skinId,
        string imageUrl,
        string marketUrl,
        string title,
        int archiveId,
        int groupId,
        int count,
        decimal buyPrice,
        decimal soldPrice,
        decimal soldSum,
        string currencyMark,
        double change,
        DateTime buyDate,
        DateTime soldDate) : base(skinId, imageUrl, marketUrl, title)
    {
        ArchiveId = archiveId;
        GroupId = groupId;

        Count = count;

        BuyPrice = buyPrice;
        SoldPrice = soldPrice;

        BuyPriceString = $"{buyPrice:N2} {currencyMark}";
        SoldPriceString = $"{soldPrice:N2} {currencyMark}";
        SoldSumString = $"{soldSum:N2} {currencyMark}";

        Change = change;

        BuyDate = buyDate;
        SoldDate = soldDate;

        BuyDateString = buyDate.ToString(ProgramConstants.VIEW_DATE_FORMAT);
        SoldDateString = soldDate.ToString(ProgramConstants.VIEW_DATE_FORMAT);
    }

    #endregion Constructor
}
