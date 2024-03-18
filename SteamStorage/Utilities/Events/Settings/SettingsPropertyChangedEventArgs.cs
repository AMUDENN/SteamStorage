using System;

namespace SteamStorage.Utilities.Events.Settings;

public class SettingsPropertyChangedEventArgs : EventArgs
{
    #region Properties

    public string? Property { get; }

    #endregion Properties

    #region Constructor

    public SettingsPropertyChangedEventArgs(
        string? property)
    {
        Property = property;
    }

    #endregion Constructor
}
