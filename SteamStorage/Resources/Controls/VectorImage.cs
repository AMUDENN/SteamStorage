using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace SteamStorage.Resources.Controls;

public class VectorImage : TemplatedControl
{
    #region PropertiesDeclaration

    public static readonly StyledProperty<IBrush?> FillProperty =
        AvaloniaProperty.Register<AdvancedNumericUpDown, IBrush?>(nameof(Fill));

    #endregion PropertiesDeclaration

    #region Properties

    public IBrush? Fill
    {
        get => GetValue(FillProperty);
        set => SetValue(FillProperty, value);
    }

    #endregion Properties
}