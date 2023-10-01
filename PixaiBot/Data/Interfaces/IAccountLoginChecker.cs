using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.Data.Models;

namespace PixaiBot.Data.Interfaces;

public interface IAccountLoginChecker
{
    bool CheckAccountLogin(UserAccount userAccount,IToastNotificationSender toastNotificationSender);

    int CheckAllAccountsLogin(IList<UserAccount> accountsList,IToastNotificationSender toastNotificationSender = null);
}