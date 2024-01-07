using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PixaiBot.Bussines_Logic;
using PixaiBot.Data.Models;

namespace PixaiBot.Data.Interfaces;

public interface ICreditClaimer
{
    /// <summary>
    /// Occurs when credits are claimed for an account.
    /// </summary>
    public event EventHandler<UserAccount> CreditsClaimed;

    /// <summary>
    /// Occurs when the process is started for an account.
    /// </summary>
    public event EventHandler<UserAccount> ProcessStartedForAccount;

    /// <summary>
    /// Claims credits for an account.
    /// </summary>
    /// <param name="account">The account where the credits are to be claimed</param>
    public void ClaimCredits(UserAccount account);
    
    /// <summary>
    ///  Claims credits for all accounts in <paramref name="accounts"/>.
    /// </summary>
    /// <param name="accounts">List of the accounts.</param>
    /// <param name="cancellationToken">token to cancel operation.</param>
    public void ClaimCreditsForAllAccounts(IEnumerable<UserAccount> accounts, CancellationToken cancellationToken);
}