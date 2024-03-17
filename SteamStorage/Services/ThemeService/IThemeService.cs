using Avalonia.Styling;
using SteamStorage.Utilities.Events;
using SteamStorage.Utilities.Events.Settings;
using SteamStorage.Utilities.ThemeVariants;

namespace SteamStorage.Services.ThemeService;

public interface IThemeService
{
    public delegate void ThemeChangedEventHandler(object? sender, ThemeChangedEventArgs args);
    
    public delegate void ChartThemeChangedEventHandler(object? sender, ChartThemeChangedEventArgs args);

    public event ThemeChangedEventHandler? ThemeChanged;
    
    public event ChartThemeChangedEventHandler? ChartThemeChanged;

    public ThemeVariant CurrentThemeVariant { get; }

    public ChartThemeVariant CurrentChartThemeVariant { get; }

    public void ChangeTheme(ThemeVariant themeVariant);
}
