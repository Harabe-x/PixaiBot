using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PixaiBot.UI.Models;

namespace PixaiBot.Data.Interfaces;

public interface ICreditClaimer
{
    /// <summary>
    /// Occurs when an error occurs.
    /// </summary>
    public event EventHandler<string> ErrorOccurred;

    /// <summary>
    /// Occurs when credits are claimed for an userAccount.
    /// </summary>
    public event EventHandler<UserAccount> CreditsClaimed;

    /// <summary>
    /// Occurs when the process is started for an userAccount.
    /// </summary>
    public event EventHandler<UserAccount> ProcessStartedForAccount;


    /// <summary>
    ///  Occurred when credits are already claimed for an userAccount.
    /// </summary>
    public event EventHandler CreditsAlreadyClaimed;

    /// <summary>
    /// Claims credits for an userAccount.
    /// </summary>
    /// <param name="userAccount">The userAccount where the credits are to be claimed</param>
    public void ClaimCredits(UserAccount userAccount);

    /// <summary>
    ///  Claims credits for all accounts in <paramref name="accounts"/>.
    /// </summary>
    /// <param name="accounts">List of the accounts.</param>
    /// <param name="cancellationToken">token to cancel operation.</param>
    public void ClaimCreditsForAllAccounts(IEnumerable<UserAccount> accounts, CancellationToken cancellationToken);
}