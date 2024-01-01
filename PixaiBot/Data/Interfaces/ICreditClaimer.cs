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
    public event EventHandler<UserAccount> CreditsClaimed;

    public event EventHandler<UserAccount> ProcessStartedForAccount;

    public void ClaimCredits(UserAccount account);

    public void ClaimCreditsForAllAccounts(IEnumerable<UserAccount> accounts, CancellationToken cancellationToken);
}