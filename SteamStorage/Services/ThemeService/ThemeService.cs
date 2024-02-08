using Avalonia;
using Avalonia.Styling;

namespace SteamStorage.Services.ThemeService;

public class ThemeService : IThemeService
{
    #region Events

    public event IThemeService.ThemeChangedEventHandler? ThemeChanged;

    #endregion Events

    #region Methods

    public void ChangeTheme(ThemeVariant? themeVariant)
    {
        Application? app = Application.Current;
        ThemeVariant? currentTheme = app?.RequestedThemeVariant;
        if (app is null) return;
        app.RequestedThemeVariant = themeVariant;
        OnThemeChanged(currentTheme, app.RequestedThemeVariant);
    }

    private void OnThemeChanged(ThemeVariant? oldTheme, ThemeVariant? newTheme)
    {
        ThemeChanged?.Invoke(this, new(oldTheme, newTheme));
    }

    #endregion Methods
}