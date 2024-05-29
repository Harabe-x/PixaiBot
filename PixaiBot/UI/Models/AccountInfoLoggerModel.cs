using PixaiBot.Data.Interfaces;

namespace PixaiBot.UI.Models;

internal class AccountInfoLoggerModel : IAccountInfoLoggerSettings
{
    public AccountInfoLoggerModel()
    {
        ShouldLogAccountId = true;
        ShouldLogAccountUsername = true;
        ShouldLogEmailVerificationStatus = true;
        ShouldLogFollowersCount = true;
        ShouldLogFollowingCount = true;
        ShouldLogAccountCredits = true;
    }

    public bool IsRunning { get; set; }

    public string OperationStatus { get; set; }

    public string LogButtonText { get; set; }
    public bool ShouldLogEmailVerificationStatus { get; set; }

    public bool ShouldLogFollowingCount { get; set; }

    public bool ShouldLogFollowersCount { get; set; }

    public bool ShouldLogAccountId { get; set; }

    public bool ShouldLogAccountUsername { get; set; }

    public bool ShouldLogAccountCredits { get; set; }
}