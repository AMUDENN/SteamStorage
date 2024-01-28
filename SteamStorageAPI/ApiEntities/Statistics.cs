namespace SteamStorageAPI.ApiEntities;

public static class Statistics
{
    #region Records

    public record InvestmentSumResponse(double TotalSum, double PercentageGrowth);

    public record FinancialGoalResponse(double FinancialGoal, double PercentageCompletion);

    public record ActiveStatisticResponse(int Count, double CurrentSum, double PercentageGrowth);

    public record ArchiveStatisticResponse(int Count, double SoldSum, double PercentageGrowth);

    public record InventoryStatisticResponse(int Count, double Sum, IEnumerable<InventoryGameStatisticResponse> Games);

    public record InventoryGameStatisticResponse(string GameTitle, double Percentage);

    public record ItemsCountResponse(int Count);

    #endregion Records
}