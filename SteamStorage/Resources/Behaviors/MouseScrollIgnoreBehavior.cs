using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace SteamStorage.Resources.Behaviors;

public class MouseScrollIgnoreBehavior : Behavior<Control>
{
    #region Methods

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject?.AddHandler(InputElement.PointerWheelChangedEvent, PointerWheelChangedHandler, RoutingStrategies.Bubble);
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject?.RemoveHandler(InputElement.PointerWheelChangedEvent, PointerWheelChangedHandler);
    }

    private void PointerWheelChangedHandler(object? sender, PointerWheelEventArgs e)
    {
        e.Handled = true;
    }

    #endregion Methods
}
