using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using SteamStorage.Services.ClipboardService;
using SteamStorage.Services.NotificationService;

namespace SteamStorage.Resources.Controls.Behaviors;

public class TextBlockBehavior : Behavior<TextBlock>
{
    #region Methods

    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject is null)
            return;
        AssociatedObject.PointerPressed += PointerPressedHandler;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        if (AssociatedObject is null)
            return;
        AssociatedObject.PointerPressed -= PointerPressedHandler;
    }

    private async void PointerPressedHandler(object? sender, PointerPressedEventArgs e)
    {
        if (AssociatedObject?.Text is null)
            return;
        IClipboardService? clipboardService = App.GetService<IClipboardService>();
        if (clipboardService is null)
            return;
        await clipboardService.SaveToClipboard(AssociatedObject.Text);
        INotificationService? notificationService = App.GetService<INotificationService>();
        if (notificationService is null)
            return;
        await notificationService.ShowAsync("Текст скопирован!", AssociatedObject.Text);
    }

    #endregion Methods
}
