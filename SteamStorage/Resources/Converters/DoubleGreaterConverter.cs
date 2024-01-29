using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace SteamStorage.Resources.Converters;

public class DoubleGreaterConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            return (double)value! > System.Convert.ToDouble(parameter);
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
