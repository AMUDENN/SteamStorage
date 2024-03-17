using System;
using SteamStorage.Utilities.ThemeVariants;

namespace SteamStorage.Utilities.Events.Settings;


public class ChartThemeChangedEventArgs : EventArgs
{
    #region Properties

    public ChartThemeVariant? OldTheme { get; }
    public ChartThemeVariant? NewTheme { get; }

    #endregion Properties

    #region Constructor

    public ChartThemeChangedEventArgs(
        ChartThemeVariant? oldTheme, 
        ChartThemeVariant? newTheme)
    {
        OldTheme = oldTheme;
        NewTheme = newTheme;
    }

    #endregion Constructor
}
