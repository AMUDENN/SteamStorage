using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace SteamStorage.Resources.Converters;

public class DoubleGreaterConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null || parameter is null) return false;
        try
        {
            return System.Convert.ToDouble(value) > System.Convert.ToDouble(parameter);
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
