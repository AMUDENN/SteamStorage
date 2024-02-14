using System.Collections.Generic;
using SkiaSharp;

namespace SteamStorage.Utilities.ThemeVariants;

public static class ChartThemeVariants
{
    #region Properties

    public static IEnumerable<ChartThemeVariant> ChartThemes { get; }

    public static ChartThemeVariant Default { get; } = new("Default", new List<SKColor>
    {
        new(80, 80, 80),
        new(90, 90, 90),
        new(100, 100, 90),
        new(110, 110, 100),
        new(120, 120, 110)
    });

    private static ChartThemeVariant Classic { get; } = new(nameof(ThemeConstants.Themes.Classic), new List<SKColor>
    {
        new(157, 105, 242),
        new(199, 112, 245),
        new(210, 218, 221)
    });

    private static ChartThemeVariant Lime { get; } = new(nameof(ThemeConstants.Themes.Lime), new List<SKColor>
    {
        new(157, 105, 242),
        new(199, 112, 245),
        new(210, 218, 221)
    });

    #endregion Properties

    #region Constructor

    static ChartThemeVariants()
    {
        ChartThemes = new List<ChartThemeVariant>
        {
            Default, Classic, Lime
        };
    }

    #endregion Constructor
}
