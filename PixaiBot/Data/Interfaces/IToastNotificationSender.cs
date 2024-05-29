using System;
using Notification.Wpf;

namespace PixaiBot.Data.Interfaces;

public interface IToastNotificationSender
{
    /// <summary>
    ///     Sends a notification to the user.
    /// </summary>
    /// <param name="title">Notification title</param>
    /// <param name="message">
    ///     Notification message
    ///     <param>
    ///         <param name="notificationType">Notification type defined by <see cref="NotificationType" /> </param>
    ///         <param name="onClick">Notification event invoked when clicked</param>
    public void SendNotification(string title, string message, NotificationType notificationType,
        Action onClick = null);
}