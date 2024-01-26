namespace SteamStorageAPI.Events;

public class TokenChangedEventArgs : EventArgs
{
    #region Fields

    public bool IsTokenEmpty { get; set; }

    #endregion Fields

    #region Constructor

    public TokenChangedEventArgs(string token)
    {
        IsTokenEmpty = string.IsNullOrEmpty(token);
    }

    #endregion Constructor

}