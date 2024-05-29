using Notification.Wpf;

namespace PixaiBot.UI.Models;

public class Notification
{
    public string Message { get; set; }

    public NotificationType NotificationType { get; set; }
}