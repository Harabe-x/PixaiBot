namespace PixaiBot.UI.Models;

internal class SettingsModel
{
    public UserConfig UserConfig { get; set; }

    public bool IsAccountCheckerRunning { get; set; }

    public string AccountCheckerButtonText;
}