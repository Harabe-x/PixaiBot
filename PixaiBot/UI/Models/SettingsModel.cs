using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.Data.Models;

namespace PixaiBot.UI.View
{
    class SettingsModel
    {
        public UserConfig UserConfig { get; set; }

        public bool IsAccountCheckerRunning { get; set; }

        public string AccountCheckerButtonText;
    }
}
