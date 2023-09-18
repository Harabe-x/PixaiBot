using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notification.Wpf;
using PixaiBot.Data.Interfaces;

namespace PixaiBot.Bussines_Logic
{
    internal class ToastNotificationSender : IToastNotificationSender
    {

        private readonly NotificationManager _notification;

        public ToastNotificationSender()
        {
            _notification = new NotificationManager();
        }
        
        public void SendNotification(string title,string message,NotificationType notificationType)
        {
          _notification.Show( title,message,notificationType);

        }
    }
}
