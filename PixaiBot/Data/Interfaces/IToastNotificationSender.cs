using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notification.Wpf;

namespace PixaiBot.Data.Interfaces;

public interface IToastNotificationSender
{
    public void SendNotification(string title, string message, NotificationType notificationType);
}