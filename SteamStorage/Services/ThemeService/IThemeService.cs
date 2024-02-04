using Avalonia.Styling;
using SteamStorage.Utilities.Events;

namespace SteamStorage.Services.ThemeService;

public interface IThemeService
{
    public delegate void ThemeChangedEventHandler(object? sender, ThemeChangedEventArgs args);
    
    public event ThemeChangedEventHandler? ThemeChanged;
    
    public void ChangeTheme(ThemeVariant? themeVariant);
}