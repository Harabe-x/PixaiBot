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

        //TODO: write the logic for logging the accounts info

        public void StartLoggingAccountsInfo(IEnumerable<UserAccount> userAccountsList, AccountInfoLoggerModel model,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
