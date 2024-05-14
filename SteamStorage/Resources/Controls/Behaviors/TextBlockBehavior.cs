using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using SteamStorage.Services.ClipboardService;
using SteamStorage.Services.NotificationService;

namespace SteamStorage.Resources.Controls.Behaviors;

public class TextBlockBehavior : Behavior<TextBlock>
{
    #region PropertiesDeclaration

    public static readonly AttachedProperty<string> PressedClassProperty =
        AvaloniaProperty.RegisterAttached<TextBlockBehavior, Interactive, string>(
            nameof(PressedClass),
            string.Empty,
            false,
            BindingMode.OneTime);

    #endregion PropertiesDeclaration

    #region Properties

    public string PressedClass
    {
        get => GetValue(PressedClassProperty);
        set => SetValue(PressedClassProperty, value);
    }

    #endregion Properties

    #region Methods

    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject is null)
            return;
        AssociatedObject.PointerPressed += PointerPressedHandler;
        AssociatedObject.PointerReleased += PointerReleasedHandler;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        if (AssociatedObject is null)
            return;
        AssociatedObject.PointerPressed -= PointerPressedHandler;
        AssociatedObject.PointerReleased -= PointerReleasedHandler;
    }

    private async void PointerPressedHandler(object? sender, PointerPressedEventArgs e)
    {
        PointerPoint point = e.GetCurrentPoint(sender as Control);
        if (point.Properties.IsRightButtonPressed || AssociatedObject?.Text is null)
            return;
        AssociatedObject.Classes.Add(PressedClass);
        IClipboardService? clipboardService = App.GetService<IClipboardService>();
        if (clipboardService is null)
            return;
        await clipboardService.SaveToClipboard(AssociatedObject.Text);
        INotificationService? notificationService = App.GetService<INotificationService>();
        if (notificationService is null)
            return;
        await notificationService.ShowAsync("Текст скопирован!", AssociatedObject.Text);
    }

    private void PointerReleasedHandler(object? sender, PointerReleasedEventArgs e)
    {
        if (AssociatedObject?.Text is null)
            return;
        AssociatedObject.Classes.Remove(PressedClass);
    }

    #endregion Methods
}
