using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace SteamStorage.Resources.Converters;

public class DoubleMultiplicationConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null || parameter is null) return false;
        try
        {
            NumberFormatInfo nfi = new()
            {
                NumberDecimalSeparator = "."
            };
            return System.Convert.ToDouble(value) * System.Convert.ToDouble(parameter, nfi);
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