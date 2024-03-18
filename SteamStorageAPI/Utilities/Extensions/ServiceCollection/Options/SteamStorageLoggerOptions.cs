namespace SteamStorageAPI.Utilities.Extensions.ServiceCollection.Options;

public class SteamStorageLoggerOptions
{
    #region Properties

    public string ProgramName { get; set; } = "SteamStorageAPI";

    public string DateFormat { get; set; } = "yy.MM.dd hh:mm:ss";

    public string DateTimeFormat { get; set; } = "yyMMdd";

    public int LogFilesLifetime { get; set; } = 30;

    #endregion Properties
}
