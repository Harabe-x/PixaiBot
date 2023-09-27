using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixaiBot.Data.Interfaces;

public interface ITrayIconHelper
{
    public bool CanHideToTray();

    public Action HideToTray { get; set; }
}