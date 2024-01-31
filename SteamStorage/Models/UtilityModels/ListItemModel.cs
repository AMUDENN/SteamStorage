namespace SteamStorage.Models.UtilityModels;

public class ListItemModel
{
    #region Properties
    
    public int Id { get; }
    public string ImageUrl { get; }
    public string Title { get; }
    public decimal CurrentPrice { get; }
    public string CurrencyMark { get; }
    public double Change7D { get; }
    public double Change30D { get; }
    public bool IsMarked { get; }

    #endregion Properties

    #region Constructor

    public ListItemModel(int id, string imageUrl, string title, decimal currentPrice, string currencyMark, double change7D, double change30D, bool isMarked)
    {
        Id = id;
        ImageUrl = imageUrl;
        Title = title;
        CurrentPrice = currentPrice;
        CurrencyMark = currencyMark;
        Change7D = change7D;
        Change30D = change30D;
        IsMarked = isMarked;
    }

    #endregion Constructor
}