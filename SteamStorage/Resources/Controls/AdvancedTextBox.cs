using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace SteamStorage.Resources.Controls;

public class AdvancedTextBox : TextBox
{
    #region PropertiesDeclaration

    public static readonly StyledProperty<string?> PreviewCharRegexProperty =
        AvaloniaProperty.Register<AdvancedTextBox, string?>(nameof(PreviewCharRegex));

    #endregion PropertiesDeclaration

    #region Properties

    public string? PreviewCharRegex
    {
        get => GetValue(PreviewCharRegexProperty);
        set
        {
            SetValue(PreviewCharRegexProperty, value);
            if (value is not null) PreviewCharRegexExpression = new(value);
        }
    }

    private Regex? PreviewCharRegexExpression { get; set; }

    #endregion Properties

    #region Constructor

    public AdvancedTextBox()
    {
        KeyDown += KeyDownHandler;

        PastingFromClipboard += PastingFromClipboardHandler;
    }

    #endregion Constructor

    #region Methods

    private void KeyDownHandler(object? sender, KeyEventArgs e)
    {
        if (PreviewCharRegexExpression is not null && e.KeySymbol is not null)
            e.Handled = !PreviewCharRegexExpression.IsMatch(e.KeySymbol);
    }

    private void PastingFromClipboardHandler(object? sender, RoutedEventArgs e)
    {
        e.Handled = true;
    }

    #endregion Methods
}
