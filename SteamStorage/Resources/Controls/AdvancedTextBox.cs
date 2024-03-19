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

    public static readonly StyledProperty<string?> StringFormatProperty =
        AvaloniaProperty.Register<AdvancedTextBox, string?>(nameof(StringFormat));

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

    public string? StringFormat
    {
        get => GetValue(StringFormatProperty);
        set => SetValue(StringFormatProperty, value);
    }

    private Regex? PreviewCharRegexExpression { get; set; }

    #endregion Properties

    #region Constructor

    public AdvancedTextBox()
    {
        KeyDown += KeyDownHandler;

        LostFocus += LostFocusHandler;

        PastingFromClipboard += PastingFromClipboardHandler;
    }

    #endregion Constructor

    #region Methods

    private void KeyDownHandler(object? sender, KeyEventArgs e)
    {
        if (PreviewCharRegexExpression is not null && e.KeySymbol is not null)
            e.Handled = !PreviewCharRegexExpression.IsMatch(e.KeySymbol);
    }

    private void LostFocusHandler(object? sender, RoutedEventArgs e)
    {
        if (StringFormat is null) return;

        bool result = int.TryParse(Text, out int intNumber);
        if (result) Text = intNumber.ToString(StringFormat);

        result = double.TryParse(Text, out double doubleNumber);
        if (result) Text = doubleNumber.ToString(StringFormat);
    }

    private void PastingFromClipboardHandler(object? sender, RoutedEventArgs e)
    {
        e.Handled = true;
    }

    #endregion Methods
}
