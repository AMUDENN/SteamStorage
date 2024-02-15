using System;
using System.Linq;
using Avalonia;
using Avalonia.Styling;
using SteamStorage.Utilities.ThemeVariants;

namespace SteamStorage.Services.ThemeService;

public class ThemeService : IThemeService
{
    #region Events

    public event IThemeService.ThemeChangedEventHandler? ThemeChanged;

    public event IThemeService.ChartThemeChangedEventHandler? ChartThemeChanged;

    #endregion Events

    #region Properties

    public ThemeVariant CurrentThemeVariant { get; private set; }

    public ChartThemeVariant CurrentChartThemeVariant { get; private set; }

    #endregion Properties

    #region Constructor

    public ThemeService()
    {
        CurrentThemeVariant = ThemeVariant.Default;
        CurrentChartThemeVariant = ChartThemeVariants.Default;

        Application? app = Application.Current;
        if (app is null) return;
        app.ActualThemeVariantChanged += ActualThemeVariantChangedHandler;
    }

    #endregion Constructor

    #region Methods

    private void ActualThemeVariantChangedHandler(object? sender, EventArgs e)
    {
        Application? app = Application.Current;
        ThemeVariant? currentTheme = app?.RequestedThemeVariant;
        if (currentTheme is null) return;

        ChartThemeVariant currentChartTheme = CurrentChartThemeVariant;

        CurrentChartThemeVariant =
            ChartThemeVariants.ChartThemes.FirstOrDefault(x => x.ToString() == currentTheme.ToString()) ??
            ChartThemeVariants.Default;

        OnChartThemeChanged(currentChartTheme, CurrentChartThemeVariant);
    }

    public void ChangeTheme(ThemeVariant themeVariant)
    {
        Application? app = Application.Current;
        ThemeVariant? currentTheme = app?.RequestedThemeVariant;
        if (app is null) return;
        app.RequestedThemeVariant = themeVariant;

        CurrentThemeVariant = themeVariant;

        OnThemeChanged(currentTheme, app.RequestedThemeVariant);
    }

    private void OnThemeChanged(ThemeVariant? oldTheme, ThemeVariant? newTheme)
    {
        ThemeChanged?.Invoke(this, new(oldTheme, newTheme));
    }

    private void OnChartThemeChanged(ChartThemeVariant? oldTheme, ChartThemeVariant? newTheme)
    {
        ChartThemeChanged?.Invoke(this, new(oldTheme, newTheme));
    }

    #endregion Methods
}
