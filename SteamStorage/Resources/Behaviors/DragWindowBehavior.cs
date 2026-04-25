using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace SteamStorage.Resources.Behaviors;

public class DragWindowBehavior : Behavior<Control>
{
    #region Methods

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject?.AddHandler(InputElement.PointerPressedEvent, PointerPressedHandler, RoutingStrategies.Bubble);
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject?.RemoveHandler(InputElement.PointerPressedEvent, PointerPressedHandler);
    }

    private void PointerPressedHandler(object? sender, PointerPressedEventArgs e)
    {
        if (TopLevel.GetTopLevel(AssociatedObject) is Window window)
            window.BeginMoveDrag(e);
    }

    #endregion Methods
}
