namespace PixaiBot.UI.Models;

public class UserConfig
{
    public bool StartWithSystem { get; set; }

    public bool ToastNotifications { get; set; }

    public bool CreditsAutoClaim { get; set; }

    public bool MultiThreading { get; set; }

    public int NumberOfThreads { get; set; }
}