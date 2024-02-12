using Avalonia;
using Avalonia.Controls;
using Avalonia.Utilities;
using CommunityToolkit.Mvvm.Input;

namespace SteamStorage.Resources.Controls;

public class AdvancedNumericUpDown : NumericUpDown
{
    #region PropertiesDeclaration

    public static readonly StyledProperty<bool> IsStartEnabledProperty =
        AvaloniaProperty.Register<AdvancedNumericUpDown, bool>(nameof(IsStartEnabled));

    public static readonly StyledProperty<bool> IsEndEnabledProperty =
        AvaloniaProperty.Register<AdvancedNumericUpDown, bool>(nameof(IsEndEnabled));

    public static readonly StyledProperty<RelayCommand?> GoToStartCommandProperty =
        AvaloniaProperty.Register<AdvancedNumericUpDown, RelayCommand?>(nameof(GoToStartCommand));

    public static readonly StyledProperty<RelayCommand?> GoToEndCommandProperty =
        AvaloniaProperty.Register<AdvancedNumericUpDown, RelayCommand?>(nameof(GoToEndCommand));

    public static readonly StyledProperty<RelayCommand?> DecrementCommandProperty =
        AvaloniaProperty.Register<AdvancedNumericUpDown, RelayCommand?>(nameof(DecrementCommand));

    public static readonly StyledProperty<RelayCommand?> IncrementCommandProperty =
        AvaloniaProperty.Register<AdvancedNumericUpDown, RelayCommand?>(nameof(IncrementCommand));

    #endregion PropertiesDeclaration

    #region Commands

    public bool IsStartEnabled
    {
        get => GetValue(IsStartEnabledProperty);
        private set => SetValue(IsStartEnabledProperty, value);
    }

    public bool IsEndEnabled
    {
        get => GetValue(IsEndEnabledProperty);
        private set => SetValue(IsEndEnabledProperty, value);
    }

    public RelayCommand? GoToStartCommand
    {
        get => GetValue(GoToStartCommandProperty);
        private set => SetValue(GoToStartCommandProperty, value);
    }

    public RelayCommand? GoToEndCommand
    {
        get => GetValue(GoToEndCommandProperty);
        private set => SetValue(GoToEndCommandProperty, value);
    }

    public RelayCommand? DecrementCommand
    {
        get => GetValue(DecrementCommandProperty);
        private set => SetValue(DecrementCommandProperty, value);
    }

    public RelayCommand? IncrementCommand
    {
        get => GetValue(IncrementCommandProperty);
        private set => SetValue(IncrementCommandProperty, value);
    }

    #endregion Commands

    #region Constructor

    public AdvancedNumericUpDown()
    {
        Initialized += (_, _) =>
        {
            GoToStartCommand = new(DoGoToStart);
            GoToEndCommand = new(DoGoToEnd);
            DecrementCommand = new(DoDecrement);
            IncrementCommand = new(DoIncrement);
        };

        ValueChanged += ValueChangedHandler;
    }

    #endregion Constructor

    #region Methods

    private void ValueChangedHandler(object? sender, NumericUpDownValueChangedEventArgs e)
    {
        IsStartEnabled = true;
        IsEndEnabled = true;
        if (Value == Minimum)
            IsStartEnabled = false;
        if (Value == Maximum)
            IsEndEnabled = false;
    }

    private void DoGoToEnd()
    {
        SetCurrentValue(ValueProperty, MathUtilities.Clamp(Maximum, Minimum, Maximum));
    }

    private void DoGoToStart()
    {
        SetCurrentValue(ValueProperty, MathUtilities.Clamp(Minimum, Minimum, Maximum));
    }

    private void DoIncrement()
    {
        decimal result;
        if (Value.HasValue)
            result = Value.Value + Increment;
        else
            result = IsSet(MinimumProperty) ? Minimum : 0;

        SetCurrentValue(ValueProperty, MathUtilities.Clamp(result, Minimum, Maximum));
    }

    private void DoDecrement()
    {
        decimal result;
        if (Value.HasValue)
            result = Value.Value - Increment;
        else
            result = IsSet(MaximumProperty) ? Maximum : 0;

        SetCurrentValue(ValueProperty, MathUtilities.Clamp(result, Minimum, Maximum));
    }

    #endregion Methods
}
