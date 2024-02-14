using System;
using System.Collections.Generic;
using SkiaSharp;

namespace SteamStorage.Utilities.ThemeVariants;

public class ChartThemeVariant
{
    private object Key { get; }
    
    public IEnumerable<SKColor> Colors { get; private set; }
    
    public ChartThemeVariant(object key, IEnumerable<SKColor> colors)
    {
        Key = key ?? throw new ArgumentNullException(nameof(key));
        Colors = colors;
    }
    
    public override string ToString()
    {
        return Key.ToString() ?? $"ChartThemeVariant {{ Key = {Key} }}";
    }
}