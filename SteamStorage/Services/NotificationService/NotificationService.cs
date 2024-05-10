using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using SteamStorage.Views.Windows;

namespace SteamStorage.Services.NotificationService;

public class NotificationService : INotificationService
{
    #region Fields

    private readonly WindowNotificationManager _notificationManager;

    #endregion Fields

    #region Constructor

    public NotificationService(
        MainWindow mainWindow)
    {
        _notificationManager = new(TopLevel.GetTopLevel(mainWindow));
    }

    #endregion Constructor

    #region Methods

    public async Task ShowAsync(
        string? title,
        string? content,
        NotificationType type = NotificationType.Information,
        TimeSpan? expiration = null,
        Action? onClick = null,
        Action? onClose = null,
        CancellationToken cancellationToken = default)
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
            _notificationManager.Show(new Notification(
                title,
                content,
                type,
                expiration,
                onClick,
                onClose
            )));
    }

    #endregion Methods
}
