using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PixaiBot.Data.Models;

namespace PixaiBot.Data.Interfaces;

public interface IAccountLoginChecker
{


    event EventHandler<UserAccount> ValidAccountLogin;

    bool CheckAccountLogin(UserAccount userAccount);

    IEnumerable<UserAccount> CheckAllAccountsLogin(IEnumerable<UserAccount> accountsList,CancellationToken token);
}