using Avalonia.Media;

namespace SteamStorage.Utilities.Extensions;

public static class ColorExtension
{
    public static string ToHexColor(this Color color)
    {
        return color.A.ToString("X2") + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
    }
}
