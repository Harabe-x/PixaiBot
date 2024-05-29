using System;
using System.Collections.Generic;
using System.Threading;
using PixaiBot.Business_Logic.Driver_and_Browser_Management.Driver_Creation_Strategy;
using PixaiBot.UI.Models;

namespace PixaiBot.Data.Interfaces;

public interface ICreditClaimer
{
    /// <summary>
    ///     Occurs when an error occurs.
    /// </summary>
    public event EventHandler<string> ErrorOccurred;

    /// <summary>
    ///     Occurs when credits are claimed for an userAccount.
    /// </summary>
    public event EventHandler<UserAccount> CreditsClaimed;

    /// <summary>
    ///     Occurs when the process is started for an userAccount.
    /// </summary>
    public event EventHandler<UserAccount> ProcessStartedForAccount;

    /// <summary>
    ///     Claims credits for an userAccount.
    /// </summary>
    /// <param name="userAccount">The userAccount where the credits are to be claimed</param>
    /// <param name="driverCreationStrategy">Driver creation strategy.</param>
    public void ClaimCredits(UserAccount userAccount, IDriverCreationStrategy driverCreationStrategy);

    /// <summary>
    ///     Claims credits for all accounts in <paramref name="accounts" />.
    /// </summary>
    /// <param name="accounts">List of the accounts.</param>
    /// <param name="driverCreationStrategy">Driver creation strategy.</param>
    /// <param name="cancellationToken">token to cancel operation.</param>
    public void ClaimCreditsForAllAccounts(IEnumerable<UserAccount> accounts,
        IDriverCreationStrategy driverCreationStrategy, CancellationToken cancellationToken);
}