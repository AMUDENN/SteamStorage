using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using SteamStorage.Models.Tools;
using SteamStorage.Services.ThemeService;
using SteamStorage.Utilities.Events.Settings;
using SteamStorage.Utilities.ThemeVariants;

namespace SteamStorage.Models;

public class ChartTooltipModel : ModelBase
{
    #region Fields

    private readonly IThemeService _themeService;

    private SolidColorPaint _tooltipTextPaint;
    private SolidColorPaint _tooltipBackgroundPaint;

    #endregion Fields

    #region Properties

    public SolidColorPaint TooltipTextPaint
    {
        get => _tooltipTextPaint;
        private set => SetProperty(ref _tooltipTextPaint, value);
    }

    public SolidColorPaint TooltipBackgroundPaint
    {
        get => _tooltipBackgroundPaint;
        private set => SetProperty(ref _tooltipBackgroundPaint, value);
    }

    #endregion Properties

    #region Constructor

    public ChartTooltipModel(
        IThemeService themeService)
    {
        _themeService = themeService;
        themeService.ChartThemeChanged += ChartThemeChangedHandler;

        _tooltipTextPaint = new SolidColorPaint();
        _tooltipBackgroundPaint = new SolidColorPaint();

        SetChartTooltip();
    }

    #endregion Constructor

    #region Methods

    private void ChartThemeChangedHandler(object? sender, ChartThemeChangedEventArgs args)
    {
        SetChartTooltip();
    }

    private void SetChartTooltip()
    {
        TooltipTextPaint = new SolidColorPaint
        {
            Color = _themeService.CurrentChartThemeVariant.GetChartColor(ChartThemeVariants.ChartColors.Foreground)
                .Color,
            SKTypeface = _themeService.CurrentChartThemeVariant.SkTypeface
        };

        SKColor background = _themeService.CurrentChartThemeVariant
            .GetChartColor(ChartThemeVariants.ChartColors.Background).Color;

        TooltipBackgroundPaint = new SolidColorPaint
        {
            Color = new SKColor(background.Red, background.Green, background.Blue, 200)
        };
    }

    #endregion Methods
}