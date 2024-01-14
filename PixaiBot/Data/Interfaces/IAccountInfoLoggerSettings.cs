using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixaiBot.Data.Interfaces;

internal interface IAccountInfoLoggerSettings
{
    /// <summary>
    ///   Gets or sets a value indicating whether the account info logger should log the email verification status.
    /// </summary>
    public bool ShouldLogEmailVerificationStatus { get; set; }

    /// <summary>
    ///  Gets or sets a value indicating whether the account info logger should log the following count.
    /// </summary>
    public bool ShouldLogFollowingCount { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the account info logger should log the followers count.
    /// </summary>
    public bool ShouldLogFollowersCount { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the account info logger should log the account id.
    /// </summary>
    public bool ShouldLogAccountId { get; set; }

    /// <summary>
    ///  Gets or sets a value indicating whether the account info logger should log the account username.
    /// </summary>
    public bool ShouldLogAccountUsername { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the account info logger should log the account credits.
    /// </summary>
    public bool ShouldLogAccountCredits { get; set; }
}