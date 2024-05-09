using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using SteamStorage.Utilities;

namespace SteamStorage.Resources.Controls;

public class AdvancedNumericUpDown : NumericUpDown
{
    #region PropertiesDeclaration

    public static readonly StyledProperty<bool> IsStartEnabledProperty =
        AvaloniaProperty.Register<AdvancedNumericUpDown, bool>(nameof(IsStartEnabled));

    public static readonly StyledProperty<bool> IsEndEnabledProperty =
        AvaloniaProperty.Register<AdvancedNumericUpDown, bool>(nameof(IsEndEnabled));

    public static readonly StyledProperty<ICommand?> GoToStartCommandProperty =
        AvaloniaProperty.Register<AdvancedNumericUpDown, ICommand?>(nameof(GoToStartCommand));

    public static readonly StyledProperty<ICommand?> GoToEndCommandProperty =
        AvaloniaProperty.Register<AdvancedNumericUpDown, ICommand?>(nameof(GoToEndCommand));

    public static readonly StyledProperty<ICommand?> DecrementCommandProperty =
        AvaloniaProperty.Register<AdvancedNumericUpDown, ICommand?>(nameof(DecrementCommand));

    public static readonly StyledProperty<ICommand?> IncrementCommandProperty =
        AvaloniaProperty.Register<AdvancedNumericUpDown, ICommand?>(nameof(IncrementCommand));

    #endregion PropertiesDeclaration

    #region Properties

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

    #endregion Properties

    #region Commands

    public ICommand? GoToStartCommand
    {
        get => GetValue(GoToStartCommandProperty);
        private set => SetValue(GoToStartCommandProperty, value);
    }

    public ICommand? GoToEndCommand
    {
        get => GetValue(GoToEndCommandProperty);
        private set => SetValue(GoToEndCommandProperty, value);
    }

    public ICommand? DecrementCommand
    {
        get => GetValue(DecrementCommandProperty);
        private set => SetValue(DecrementCommandProperty, value);
    }

    public ICommand? IncrementCommand
    {
        get => GetValue(IncrementCommandProperty);
        private set => SetValue(IncrementCommandProperty, value);
    }

    #endregion Commands

    #region Constructor

    public AdvancedNumericUpDown()
    {
        Initialized += InitializedHandler;

        PropertyChanged += PropertyChangedHandler;
    }

    #endregion Constructor

    #region Methods

    private void InitializedHandler(object? sender, EventArgs args)
    {
        GoToStartCommand = new RelayCommand(DoGoToStart);
        GoToEndCommand = new RelayCommand(DoGoToEnd);
        DecrementCommand = new RelayCommand(DoDecrement);
        IncrementCommand = new RelayCommand(DoIncrement);
    }

    private void PropertyChangedHandler(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property != ValueProperty && e.Property != MinimumProperty && e.Property != MaximumProperty) return;

        IsStartEnabled = true;
        IsEndEnabled = true;
        if (Value <= Minimum)
            IsStartEnabled = false;
        if (Value >= Maximum)
            IsEndEnabled = false;
    }

    private void DoGoToEnd()
    {
        SetCurrentValue(ValueProperty, MathUtility.Clamp(Maximum, Minimum, Maximum));
    }

    private void DoGoToStart()
    {
        SetCurrentValue(ValueProperty, MathUtility.Clamp(Minimum, Minimum, Maximum));
    }

    private void DoIncrement()
    {
        decimal result;
        if (Value.HasValue)
            result = Value.Value + Increment;
        else
            result = IsSet(MinimumProperty) ? Minimum : 0;

        SetCurrentValue(ValueProperty, MathUtility.Clamp(result, Minimum, Maximum));
    }

    private void DoDecrement()
    {
        decimal result;
        if (Value.HasValue)
            result = Value.Value - Increment;
        else
            result = IsSet(MaximumProperty) ? Maximum : 0;

        SetCurrentValue(ValueProperty, MathUtility.Clamp(result, Minimum, Maximum));
    }

    #endregion Methods
}
