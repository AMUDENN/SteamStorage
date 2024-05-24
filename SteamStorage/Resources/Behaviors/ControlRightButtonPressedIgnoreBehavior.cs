using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace SteamStorage.Resources.Behaviors;

public class ControlRightButtonPressedIgnoreBehavior : Behavior<Control>
{
    #region Methods

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject?.AddHandler(InputElement.PointerPressedEvent, PointerPressedHandler, RoutingStrategies.Tunnel);
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject?.RemoveHandler(InputElement.PointerPressedEvent, PointerPressedHandler);
    }

    private void PointerPressedHandler(object? sender, PointerPressedEventArgs e)
    {
        PointerPoint point = e.GetCurrentPoint(sender as Control);
        if (point.Properties.IsRightButtonPressed)
            e.Handled = true;
    }

    #endregion Methods
}
