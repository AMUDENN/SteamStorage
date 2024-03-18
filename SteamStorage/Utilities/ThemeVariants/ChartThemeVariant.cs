using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;

namespace SteamStorage.Utilities.ThemeVariants;

public class ChartThemeVariant
{
    #region Properties

    private object Key { get; }

    public SKTypeface SkTypeface { get; }

    public IEnumerable<ChartColor> Colors { get; }

    #endregion Properties

    #region Constructor

    public ChartThemeVariant(
        object key,
        SKTypeface skTypeface,
        IEnumerable<ChartColor> colors)
    {
        Key = key ?? throw new ArgumentNullException(nameof(key));
        SkTypeface = skTypeface;
        Colors = colors;
    }

    #endregion Constructor

    #region Methods

    public override string ToString()
    {
        return Key.ToString() ?? $"ChartThemeVariant {{ Key = {Key} }}";
    }

    public ChartColor GetChartColor(ChartThemeVariants.ChartColors color)
    {
        return Colors.FirstOrDefault(x => x.Name == color) ?? ChartThemeVariants.Default.GetChartColor(color);
    }

    #endregion Methods
}
