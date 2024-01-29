using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace SteamStorage.Resources.Converters;

public class PercentConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            double val = System.Convert.ToDouble(value);
            int par = System.Convert.ToInt32(parameter);
            double percent = Math.Round(val * 100, par);
            return percent > 0 ? $"+{percent}%" : $"{percent}%";
        }
        catch
        {
            return value;
        }
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}
