namespace SteamStorageAPI.Utilities.Events;

public class TokenChangedEventArgs : EventArgs
{
    #region Properties

    public string Token { get; }

    #endregion Properties

    #region Constructor

    public TokenChangedEventArgs(
        string token)
    {
        Token = token;
    }

    #endregion Constructor
}
