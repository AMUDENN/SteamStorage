using System;
using Avalonia.Styling;

namespace SteamStorage.Utilities.Events;


public class ThemeChangedEventArgs : EventArgs
{
    #region Fields

    public ThemeVariant? OldTheme { get; }
    public ThemeVariant? NewTheme { get; }

    #endregion Fields

    #region Constructor

    public ThemeChangedEventArgs(ThemeVariant? oldTheme, ThemeVariant? newTheme)
    {
        OldTheme = oldTheme;
        NewTheme = newTheme;
    }

    #endregion Constructor
}
