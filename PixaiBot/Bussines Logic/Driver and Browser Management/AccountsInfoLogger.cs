using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;

namespace PixaiBot.Bussines_Logic.Driver_and_Browser_Management
{
    internal class AccountsInfoLogger : IAccountsInfoLogger
    {
        public void StartLoggingAccountsInfo(IEnumerable<UserAccount> userAccountsList, AccountInfoLoggerSettings settings,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
