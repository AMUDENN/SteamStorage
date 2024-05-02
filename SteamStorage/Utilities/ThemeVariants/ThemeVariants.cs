using Avalonia.Styling;

namespace SteamStorage.Utilities.ThemeVariants;

public static class ThemeVariants
{
    #region Properties

    public static ThemeVariant Classic { get; } = new(nameof(ThemeConstants.Themes.Classic), ThemeVariant.Dark);

    public static ThemeVariant Lime { get; } = new(nameof(ThemeConstants.Themes.Lime), ThemeVariant.Dark);
    
    public static ThemeVariant VeryDark { get; } = new(nameof(ThemeConstants.Themes.VeryDark), ThemeVariant.Dark);
    
    public static ThemeVariant VeryLight { get; } = new(nameof(ThemeConstants.Themes.VeryLight), ThemeVariant.Light);

    #endregion Properties
}
