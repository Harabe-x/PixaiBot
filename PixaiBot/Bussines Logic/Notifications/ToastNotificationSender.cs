using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notification.Wpf;
using PixaiBot.Data.Interfaces;

namespace PixaiBot.Bussines_Logic;

public class ToastNotificationSender : IToastNotificationSender
{
    private readonly NotificationManager _notification;

    private readonly ILogger _logger;

    public ToastNotificationSender(ILogger logger)
    {
        _logger = logger;
        _notification = new NotificationManager();
    }

    public void SendNotification(string title, string message, NotificationType notificationType)
    {
        _logger.Log($"Notification Sent, NotificationType{notificationType}", _logger.ApplicationLogFilePath);
        _notification.Show(title, message, notificationType);
    }
}