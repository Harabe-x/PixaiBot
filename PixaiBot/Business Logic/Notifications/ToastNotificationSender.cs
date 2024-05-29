using System;
using Notification.Wpf;
using PixaiBot.Data.Interfaces;

namespace PixaiBot.Business_Logic.Notifications;

public class ToastNotificationSender : IToastNotificationSender
{
    private readonly ILogger _logger;
    private readonly NotificationManager _notification;

    public ToastNotificationSender(ILogger logger)
    {
        _logger = logger;
        _notification = new NotificationManager();
    }

    /// <summary>
    ///     Sends a notification to the user
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="notificationType"></param>
    /// <param name="onClick"></param>
    public void SendNotification(string title, string message, NotificationType notificationType, Action onClick = null)
    {
        _logger.Log($"Notification Sent, NotificationType{notificationType}", _logger.ApplicationLogFilePath);
        _notification.Show(title, message, notificationType, onClick: onClick);
    }
}