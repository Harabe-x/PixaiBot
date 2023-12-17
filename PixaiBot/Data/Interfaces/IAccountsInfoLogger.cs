using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PixaiBot.Data.Models;

namespace PixaiBot.Data.Interfaces
{
    internal interface IAccountsInfoLogger
    {
        public void StartLoggingAccountsInfo(IEnumerable<UserAccount> userAccountsList,IAccountInfoLoggerSettings settings,CancellationToken cancellationToken);
    }
}
