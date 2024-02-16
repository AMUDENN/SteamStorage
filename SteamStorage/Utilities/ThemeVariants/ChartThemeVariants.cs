using System.Collections.Generic;

namespace SteamStorage.Utilities.ThemeVariants;

public static class ChartThemeVariants
{
    #region Enums

    public enum ChartColors
    {
        FirstAccent, SecondAccent, ThirdAccent, FourthAccent, FifthAccent, Positive, Negative
    }
    
    #endregion Enums
    
    #region Properties

    public static IEnumerable<ChartThemeVariant> ChartThemes { get; }

    public static ChartThemeVariant Default { get; } = new(nameof(ThemeConstants.Themes.Default), new List<ChartColor>
    {
        new(ChartColors.FirstAccent, new(80, 80, 80)),
        new(ChartColors.SecondAccent, new(90, 90, 90)),
        new(ChartColors.ThirdAccent, new(100, 100, 90)),
        new(ChartColors.FourthAccent, new(110, 110, 100)),
        new(ChartColors.FifthAccent, new(120, 120, 110)),
        
        new(ChartColors.Positive, new(30, 149, 56)),
        new(ChartColors.Negative, new(224, 55, 55))
    });

    private static ChartThemeVariant Classic { get; } = new(nameof(ThemeConstants.Themes.Classic), new List<ChartColor>
    {
        new(ChartColors.FirstAccent, new(157, 105, 242)),
        new(ChartColors.SecondAccent, new(199, 112, 245)),
        new(ChartColors.ThirdAccent, new(210, 218, 221)),
        
        new(ChartColors.Positive, new(30, 149, 56)),
        new(ChartColors.Negative, new(224, 55, 55))
    });

    private static ChartThemeVariant Lime { get; } = new(nameof(ThemeConstants.Themes.Lime), new List<ChartColor>
    {
        new(ChartColors.FirstAccent, new(157, 105, 242)),
        new(ChartColors.SecondAccent, new(199, 112, 245)),
        new(ChartColors.ThirdAccent, new(210, 218, 221)),
        
        new(ChartColors.Positive, new(30, 149, 56)),
        new(ChartColors.Negative, new(224, 55, 55))
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
