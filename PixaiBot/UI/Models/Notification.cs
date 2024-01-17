using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notification.Wpf;

namespace PixaiBot.UI.Models
{
    public class Notification
    {
        public string Message { get; set; }

        public NotificationType NotificationType { get; set; }
    }
}
