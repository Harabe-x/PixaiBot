using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixaiBot.Data.Interfaces;

internal interface IAccountInfoLoggerSettings
{
    public bool ShouldLogEmailVerificationStatus { get; set; }

    public bool ShouldLogFollowingCount { get; set; }

    public bool ShouldLogFollowersCount { get; set; }

    public bool ShouldLogAccountId { get; set; }

    public bool ShouldLogAccountUsername { get; set; }

    public bool ShouldLogAccountCredits { get; set; }
}