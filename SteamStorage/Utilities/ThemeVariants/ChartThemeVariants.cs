using System.Collections.Generic;
using SkiaSharp;

namespace SteamStorage.Utilities.ThemeVariants;

public static class ChartThemeVariants
{
    #region Enums

    public enum ChartColors
    {
        FirstAccent,
        SecondAccent,
        ThirdAccent,
        FourthAccent,
        FifthAccent,

        Positive,
        Negative,

        Background,
        Foreground
    }

    #endregion Enums

    #region Properties

    public static IEnumerable<ChartThemeVariant> ChartThemes { get; }

    public static ChartThemeVariant Default { get; } = new(
        nameof(ThemeConstants.Themes.Default),
        SKTypeface.FromFamilyName(
            "Inter, sans-serif",
            SKFontStyleWeight.SemiBold,
            SKFontStyleWidth.Normal,
            SKFontStyleSlant.Upright),
        new List<ChartColor>
        {
            new(ChartColors.FirstAccent, new SKColor(80, 80, 80)),
            new(ChartColors.SecondAccent, new SKColor(90, 90, 90)),
            new(ChartColors.ThirdAccent, new SKColor(100, 100, 90)),
            new(ChartColors.FourthAccent, new SKColor(110, 110, 100)),
            new(ChartColors.FifthAccent, new SKColor(120, 120, 110)),

            new(ChartColors.Positive, new SKColor(30, 149, 56)),
            new(ChartColors.Negative, new SKColor(224, 55, 55)),

            new(ChartColors.Background, new SKColor(15, 20, 26)),
            new(ChartColors.Foreground, new SKColor(210, 218, 221))
        });

    private static ChartThemeVariant Classic { get; } = new(
        nameof(ThemeConstants.Themes.Classic),
        SKTypeface.FromFamilyName(
            "Inter, sans-serif",
            SKFontStyleWeight.SemiBold,
            SKFontStyleWidth.Normal,
            SKFontStyleSlant.Upright),
        new List<ChartColor>
        {
            new(ChartColors.FirstAccent, new SKColor(157, 105, 242)),
            new(ChartColors.SecondAccent, new SKColor(199, 112, 245)),
            new(ChartColors.ThirdAccent, new SKColor(210, 218, 221)),
            new(ChartColors.FourthAccent, new SKColor(50, 83, 192)),
            new(ChartColors.FifthAccent, new SKColor(156, 53, 173)),

            new(ChartColors.Positive, new SKColor(30, 149, 56)),
            new(ChartColors.Negative, new SKColor(224, 55, 55)),

            new(ChartColors.Background, new SKColor(15, 20, 26)),
            new(ChartColors.Foreground, new SKColor(210, 218, 221))
        });

    private static ChartThemeVariant Lime { get; } = new(
        nameof(ThemeConstants.Themes.Lime),
        SKTypeface.FromFamilyName(
            "Inter, sans-serif",
            SKFontStyleWeight.SemiBold,
            SKFontStyleWidth.Normal,
            SKFontStyleSlant.Upright),
        new List<ChartColor>
        {
            new(ChartColors.FirstAccent, new SKColor(157, 105, 242)),
            new(ChartColors.SecondAccent, new SKColor(199, 112, 245)),
            new(ChartColors.ThirdAccent, new SKColor(210, 218, 221)),
            new(ChartColors.FourthAccent, new SKColor(50, 83, 192)),
            new(ChartColors.FifthAccent, new SKColor(156, 53, 173)),

            new(ChartColors.Positive, new SKColor(30, 149, 56)),
            new(ChartColors.Negative, new SKColor(224, 55, 55)),

            new(ChartColors.Background, new SKColor(15, 20, 26)),
            new(ChartColors.Foreground, new SKColor(210, 218, 221))
        });

    private static ChartThemeVariant VeryDark { get; } = new(
        nameof(ThemeConstants.Themes.VeryDark),
        SKTypeface.FromFamilyName(
            "Inter, sans-serif",
            SKFontStyleWeight.SemiBold,
            SKFontStyleWidth.Normal,
            SKFontStyleSlant.Upright),
        new List<ChartColor>
        {
            new(ChartColors.FirstAccent, new SKColor(157, 105, 242)),
            new(ChartColors.SecondAccent, new SKColor(199, 112, 245)),
            new(ChartColors.ThirdAccent, new SKColor(210, 218, 221)),
            new(ChartColors.FourthAccent, new SKColor(50, 83, 192)),
            new(ChartColors.FifthAccent, new SKColor(156, 53, 173)),

            new(ChartColors.Positive, new SKColor(30, 149, 56)),
            new(ChartColors.Negative, new SKColor(224, 55, 55)),

            new(ChartColors.Background, new SKColor(0, 0, 0)),
            new(ChartColors.Foreground, new SKColor(231, 239, 242))
        });

    private static ChartThemeVariant VeryLight { get; } = new(
        nameof(ThemeConstants.Themes.VeryLight),
        SKTypeface.FromFamilyName(
            "Inter, sans-serif",
            SKFontStyleWeight.SemiBold,
            SKFontStyleWidth.Normal,
            SKFontStyleSlant.Upright),
        new List<ChartColor>
        {
            new(ChartColors.FirstAccent, new SKColor(157, 105, 242)),
            new(ChartColors.SecondAccent, new SKColor(199, 112, 245)),
            new(ChartColors.ThirdAccent, new SKColor(210, 218, 221)),
            new(ChartColors.FourthAccent, new SKColor(50, 83, 192)),
            new(ChartColors.FifthAccent, new SKColor(156, 53, 173)),

            new(ChartColors.Positive, new SKColor(30, 149, 56)),
            new(ChartColors.Negative, new SKColor(224, 55, 55)),

            new(ChartColors.Background, new SKColor(223, 223, 223)),
            new(ChartColors.Foreground, new SKColor(21, 24, 24))
        });

    #endregion Properties

    #region Constructor

    static ChartThemeVariants()
    {
        ChartThemes = new List<ChartThemeVariant>
        {
            Default,
            Classic,
            Lime,
            VeryDark,
            VeryLight
        };
    }

    #endregion Constructor
}