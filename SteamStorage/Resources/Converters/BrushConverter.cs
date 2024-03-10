using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace SteamStorage.Resources.Converters;

public class BrushConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string brush) return false;
        try
        {
            return SolidColorBrush.Parse(brush);
        }
        catch
        {
            return null;
        }
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}
