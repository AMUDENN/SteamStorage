using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SteamStorage.Utilities;

public static class UrlUtility
{
    internal static void OpenUrl(string url)
    {
        Process.Start(url);
        // if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        // {
        //     Process.Start(new ProcessStartInfo(url)
        //     {
        //         UseShellExecute = true
        //     });
        // }
        // else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        // {
        //     Process.Start("xdg-open", url);
        // }
        // else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        // {
        //     Process.Start("open", url);
        // }
    }
}
