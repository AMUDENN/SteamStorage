using System.Globalization;

namespace SteamStorage.Utilities.Extensions;

public static class StringExtension
{
    #region Fields

    private static readonly NumberFormatInfo _numberGroupSeparator = new()
    {
        NumberGroupSeparator = "\u00a0"
    };

    private static readonly NumberFormatInfo _numberDecimalSeparator = new()
    {
        NumberDecimalSeparator = ",",
        NumberGroupSeparator = "\u00a0"
    };

    #endregion Fields


    #region Methods

    public static bool TryParse(this string? input, out int number)
    {
        return int.TryParse(input, NumberStyles.Any, _numberGroupSeparator, out number);
    }
    
    public static bool TryParse(this string? input, out decimal number)
    {
        return decimal.TryParse(input, NumberStyles.Any, _numberDecimalSeparator, out number);
    }

    #endregion Methods
}
