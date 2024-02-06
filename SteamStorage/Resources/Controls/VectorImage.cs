using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace SteamStorage.Resources.Controls;

public class VectorImage : TemplatedControl
{
    #region PropertiesDeclaration

    public static readonly StyledProperty<IBrush?> BrushProperty =
        AvaloniaProperty.Register<AdvancedNumericUpDown, IBrush?>(nameof(Brush));

    #endregion PropertiesDeclaration

    #region Properties

    public IBrush? Brush
    {
        get => GetValue(BrushProperty);
        set => SetValue(BrushProperty, value);
    }

    #endregion Properties
}