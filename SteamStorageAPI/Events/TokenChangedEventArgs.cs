namespace SteamStorageAPI.Events;

public class TokenChangedEventArgs : EventArgs
{
    #region Fields

    public string Token { get; }

    #endregion Fields

    #region Constructor

    public TokenChangedEventArgs(string token)
    {
        Token = token;
    }

    #endregion Constructor
}
