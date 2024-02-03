namespace SteamStorageAPI.Services.PingResult;

public class PingResult
{
    #region Enums

    public enum ServerStatus
    {
        Excellent,
        Good,
        Bad,
        NoConnection
    }

    #endregion Enums

    #region Properties

    public long Ping { get; private set; }
    public ServerStatus Status { get; private set; }

    #endregion Properties

    #region Constructor

    public PingResult(long ping)
    {
        Ping = ping;
        Status = ping switch
        {
            -1 => ServerStatus.NoConnection,
            < 50 => ServerStatus.Excellent,
            < 300 => ServerStatus.Good,
            _ => ServerStatus.Bad
        };
    }

    #endregion Constructor
}
