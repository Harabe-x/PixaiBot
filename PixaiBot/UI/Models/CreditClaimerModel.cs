namespace PixaiBot.UI.Models;

internal class CreditClaimerModel
{
    public string ClaimButtonText { get; set; }

    public bool IsRunning { get; set; }

    public string OperationStatus { get; set; }

    public BotStatistics BotStatistics { get; set; }
}