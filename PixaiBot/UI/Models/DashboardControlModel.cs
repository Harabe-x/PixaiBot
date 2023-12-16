using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixaiBot.UI.Models
{
    internal class DashboardControlModel
    {
        public string AccountsCount { get; set; }

        public string BotVersion { get; set; }

        public string LastCreditClaimDateTime { get; set; }

        public string ClaimButtonText  { get; set; }

        public bool IsRunning { get; set; }

        public string BotOperationStatus { get; set; }
    }
}
