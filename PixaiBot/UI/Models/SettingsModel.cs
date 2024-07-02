namespace PixaiBot.UI.Models;

internal class SettingsModel
{
    public string AccountCheckerButtonText;

    public UserConfig UserConfig { get; set; }

    public bool IsAccountCheckerRunning { get; set; }
}