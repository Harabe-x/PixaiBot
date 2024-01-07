using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.Data.Interfaces;

namespace PixaiBot.Data.Models;

internal class AccountInfoLoggerModel : IAccountInfoLoggerSettings
{
    public bool ShouldLogEmailVerificationStatus { get; set; }

    public bool ShouldLogFollowingCount { get; set; }

    public bool ShouldLogFollowersCount { get; set; }

    public bool ShouldLogAccountId { get; set; }

    public bool ShouldLogAccountUsername { get; set; }

    public bool ShouldLogAccountCredits { get; set; }

    public bool IsRunning { get; set; }

    public string OperationStatus { get; set; }

    public string LogButtonText { get; set; }


    public AccountInfoLoggerModel()
    {
        ShouldLogAccountId = true;
        ShouldLogAccountUsername = true;
        ShouldLogEmailVerificationStatus = true;
        ShouldLogFollowersCount = true;
        ShouldLogFollowingCount = true;
        ShouldLogAccountCredits = true;
    }
}