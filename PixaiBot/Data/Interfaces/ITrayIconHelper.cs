using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixaiBot.Data.Interfaces;

public interface ITrayIconHelper
{

    /// <summary>
    ///  Determines whether the window can be shown.
    /// </summary>
    /// <returns>if window can be shown returns true; otherwise returns false</returns>
    public bool CanShowWindow();


    /// <summary>
    /// Shows the window.
    /// </summary>
    public Action ShowWindow { get; set; }

    /// <summary>
    /// Determines whether the window can be hidden to tray.
    /// </summary>
    /// <returns>If window can be hided returns true; otherwise returns false</returns>
    public bool CanHideToTray();

    public Action HideToTray { get; set; }
}