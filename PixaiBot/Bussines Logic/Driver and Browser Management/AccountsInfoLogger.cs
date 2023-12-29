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
        public AccountsInfoLogger(IPixaiNavigation pixaiNavigation, IPixaiDataReader pixaiDataReader)
        {
            _pixaiNavigation = pixaiNavigation;
            _pixaiDataReader = pixaiDataReader;
        }

        public void StartLoggingAccountsInfo(IEnumerable<UserAccount> userAccountsList,
            IAccountInfoLoggerSettings settings,
            CancellationToken cancellationToken)
        {
            foreach (var account in userAccountsList)
            {
                if (cancellationToken.IsCancellationRequested) return;
                ;

            }
        }

        private void LogAccountInfo(UserAccount account, IAccountInfoLoggerSettings settings)
        {
          using var driver = ChromeDriverFactory.CreateDriver();
          _pixaiNavigation.LogIn(driver,account.Email,account.Password);
        }
    



    #region Fields
        
        private readonly IPixaiNavigation _pixaiNavigation;

        private readonly IPixaiDataReader _pixaiDataReader;
        
        #endregion
    }
}
