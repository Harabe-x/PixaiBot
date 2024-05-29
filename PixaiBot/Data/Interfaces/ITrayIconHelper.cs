using System;

namespace PixaiBot.Data.Interfaces;

public interface ITrayIconHelper
{
    /// <summary>
    ///     Shows the window.
    /// </summary>
    public Action ShowWindow { get; set; }

    public Action HideToTray { get; set; }

    /// <summary>
    ///     Determines whether the window can be shown.
    /// </summary>
    /// <returns>if window can be shown returns true; otherwise returns false</returns>
    public bool CanShowWindow();

    /// <summary>
    ///     Determines whether the window can be hidden to tray.
    /// </summary>
    /// <returns>If window can be hided returns true; otherwise returns false</returns>
    public bool CanHideToTray();
}