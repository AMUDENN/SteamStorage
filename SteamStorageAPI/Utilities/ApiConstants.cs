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

    public const string LOG_DATETIME_FORMAT = "yy.MM.dd hh:mm:ss";
    public const string LOG_DATE_FORMAT = "yyMMdd";
    public const string LOG_PROGRAM_NAME = "SteamStorage";

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
        Roles,
        Skins,
        Statistics,
        Users
    }

    #endregion Enums
}