namespace SteamStorageAPI.Utilities;

public static class ApiConstants
{
    #region Constants

    public const string CLIENT_NAME = "MainClient";
    public const string HOST_NAME = "localhost";
    public const string SERVER_ADRESS = "http://localhost:5275/";
    public const string TOKENHUB_ADRESS = "http://localhost:5245/token-hub";
    public const string API_DATE_FORMAT = "MM.dd.yyyy";

    public const string TOKEN_METHOD_NAME = "Token";
    public const string JOIN_GROUP_METHOD_NAME = "JoinGroup";

    #endregion Constants

    #region Enums

    public enum ApiControllers
    {
        ActiveGroups,
        Actives,
        ArchiveGroups,
        Archives,
        Authorize,
        Currencies,
        Games,
        Inventory,
        Pages,
        Skins,
        Statistics,
        Users
    }

    public enum ApiMethods
    {
        GetActiveGroups = 100,
        GetActiveGroupDynamics,
        GetActiveGroupsCount,
        PostActiveGroup,
        PutActiveGroup,
        DeleteActiveGroup,

        GetActives = 200,
        GetActivesPagesCount,
        GetActivesCount,
        PostActive,
        PutActive,
        SoldActive,
        DeleteActive,

        GetArchiveGroups = 300,
        GetArchiveGroupsCount,
        PostArchiveGroup,
        PutArchiveGroup,
        DeleteArchiveGroup,

        GetArchives = 400,
        GetArchivesPagesCount,
        GetArchivesCount,
        PostArchive,
        PutArchive,
        DeleteArchive,

        GetAuthUrl = 500,

        GetCurrencies = 600,
        GetCurrency,
        SetCurrency,

        GetGames = 700,

        GetInventory = 800,
        GetInventoryPagesCount,
        GetSavedInventoriesCount,
        RefreshInventory,

        GetPages = 900,
        SetStartPage,

        GetSkinInfo = 1000,
        GetBaseSkins,
        GetSkins,
        GetSkinDynamics,
        GetSkinPagesCount,
        GetSteamSkinsCount,
        GetSavedSkinsCount,
        SetMarkedSkin,
        DeleteMarkedSkin,

        GetInvestmentSum = 1100,
        GetFinancialGoal,
        GetActiveStatistic,
        GetArchiveStatistic,
        GetInventoryStatistic,
        GetItemsCount,

        GetCurrentUserInfo = 1200,
        PutGoalSum,
        DeleteUser
    }

    #endregion Enums
}
