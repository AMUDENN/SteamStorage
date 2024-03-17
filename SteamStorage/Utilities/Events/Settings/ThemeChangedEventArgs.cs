using System;
using Avalonia.Styling;

namespace SteamStorage.Utilities.Events.Settings;


public class ThemeChangedEventArgs : EventArgs
{
    #region Properties

    public ThemeVariant? OldTheme { get; }
    public ThemeVariant? NewTheme { get; }

    #endregion Properties

    #region Constructor

    public ThemeChangedEventArgs(
        ThemeVariant? oldTheme, 
        ThemeVariant? newTheme)
    {
        OldTheme = oldTheme;
        NewTheme = newTheme;
    }

    #endregion Constructor
}
