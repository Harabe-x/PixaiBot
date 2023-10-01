using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.Bussines_Logic;
using PixaiBot.Data.Models;

namespace PixaiBot.Data.Interfaces;

public interface ICreditClaimer
{
    public event EventHandler<UserAccount> CreditClaimed;

    public void ClaimCredits(UserAccount account, IToastNotificationSender toastNotificationSender);

    public void ClaimCreditsForAllAccounts(IEnumerable<UserAccount> accounts,IToastNotificationSender toastNotificationSender = null);
}