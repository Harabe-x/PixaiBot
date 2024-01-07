using System;

namespace PixaiBot.UI.Models;

public class BotStatistics
{
    public int AccountsCount { get; set; }

    public DateTime LastCreditClaimDateTime { get; set; }

    public string? BotVersion { get; set; }
}