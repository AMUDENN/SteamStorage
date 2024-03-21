using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;

namespace SteamStorage.Resources.Controls;

public class DefaultContentControl : ContentControl
{
    #region PropertiesDeclaration

    public static readonly StyledProperty<string?> HeaderProperty =
        AvaloniaProperty.Register<DefaultContentControl, string?>(nameof(Header));
    
    public static readonly StyledProperty<object?> DefaultValueProperty =
        AvaloniaProperty.Register<DefaultContentControl, object?>(nameof(DefaultValue));

    public static readonly StyledProperty<ICommand?> ResetCommandProperty =
        AvaloniaProperty.Register<AdvancedNumericUpDown, ICommand?>(nameof(ResetCommand));

    #endregion PropertiesDeclaration

    #region Properties

    public string? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }
    
    public object? DefaultValue
    {
        get => GetValue(DefaultValueProperty);
        set => SetValue(DefaultValueProperty, value);
    }

    #endregion Properties

    #region Commands

    public ICommand? ResetCommand
    {
        get => GetValue(ResetCommandProperty);
        private set => SetValue(ResetCommandProperty, value);
    }

    #endregion Commands

    #region Constructor

    public DefaultContentControl()
    {
        Initialized += InitializedHandler;
    }

    #endregion Constructor

    #region Methods

    private void InitializedHandler(object? sender, EventArgs args)
    {
        ResetCommand = new RelayCommand(DoResetCommand);
    }

    private void DoResetCommand()
    {
        switch (Content)
        {
            case TextBox tb:
                tb.Text = DefaultValue as string ?? string.Empty;
                break;
            case ComboBox cb:
                cb.SelectedItem = DefaultValue;
                break;
            case AutoCompleteBox acb:
                acb.SelectedItem = DefaultValue;
                break;
            case DatePicker dp:
                dp.SelectedDate = DefaultValue is DateTimeOffset offset ? offset : DateTimeOffset.Now;
                break;
        }
    }

    #endregion Methods
}
