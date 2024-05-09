using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;

namespace SteamStorage.Services.NotificationService;

public interface INotificationService
{
    public Task ShowAsync(
        string? title,
        string? content,
        NotificationType type = NotificationType.Information,
        TimeSpan? expiration = null,
        Action? onClick = null,
        Action? onClose = null,
        CancellationToken cancellationToken = default);
}
