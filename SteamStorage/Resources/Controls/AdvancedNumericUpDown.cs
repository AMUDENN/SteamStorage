using Avalonia;
using Avalonia.Controls;
using Avalonia.Utilities;
using CommunityToolkit.Mvvm.Input;

namespace SteamStorage.Resources.Controls;

public class AdvancedNumericUpDown : NumericUpDown
{
    #region PropertiesDeclaration

    public static readonly StyledProperty<RelayCommand?> GoToStartCommandProperty =
        AvaloniaProperty.Register<AdvancedNumericUpDown, RelayCommand?>(nameof(GoToStartCommand));

    public static readonly StyledProperty<RelayCommand?> GoToEndCommandProperty =
        AvaloniaProperty.Register<AdvancedNumericUpDown, RelayCommand?>(nameof(Value));

    public static readonly StyledProperty<RelayCommand?> DecrementCommandProperty =
        AvaloniaProperty.Register<AdvancedNumericUpDown, RelayCommand?>(nameof(Value));

    public static readonly StyledProperty<RelayCommand?> IncrementCommandProperty =
        AvaloniaProperty.Register<AdvancedNumericUpDown, RelayCommand?>(nameof(Value));

    #endregion PropertiesDeclaration

    #region Commands

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
    }

    #endregion Constructor

    #region Methods

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