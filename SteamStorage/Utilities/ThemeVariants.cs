using Avalonia.Styling;

namespace SteamStorage.Utilities;

public static class ThemeVariants
{
    #region Properties

    public static ThemeVariant Classic { get; } = new("Classic", ThemeVariant.Dark);
    
    public static ThemeVariant Lime { get; } = new("Lime", ThemeVariant.Dark);

    #endregion Properties
}