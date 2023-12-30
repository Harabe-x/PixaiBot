using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using PixaiBot.Bussines_Logic.Driver_and_Browser_Management.WebNavigationCore.WebNavigationCoreException;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;

namespace PixaiBot.Bussines_Logic.Driver_and_Browser_Management
{
    internal class AccountsInfoLogger : IAccountsInfoLogger
    {
        public AccountsInfoLogger(IPixaiNavigation pixaiNavigation, IPixaiDataReader pixaiDataReader,ILogger logger)
        {
            _pixaiNavigation = pixaiNavigation;
            _pixaiDataReader = pixaiDataReader;
            _logger = logger; 
            _stringBuilder = new StringBuilder();
        }

        public string StartLoggingAccountsInfo(IEnumerable<UserAccount> userAccountsList,
            IAccountInfoLoggerSettings settings,
            CancellationToken cancellationToken)
        {
            _stringBuilder.Clear();

            foreach (var account in userAccountsList)
            {
                if (cancellationToken.IsCancellationRequested) return _stringBuilder.ToString();


                try
                {
                    LogAccountInfo(account, settings);
                }
                catch (Exception e)
                {
                    _logger.Log($"Error occurred, Error message : {e.Message}", _logger.ApplicationLogFilePath);
                    continue;
                }
               
               
            }

            return _stringBuilder.ToString();
        }

        private void LogAccountInfo(UserAccount account, IAccountInfoLoggerSettings settings)
        {
            using var driver = ChromeDriverFactory.CreateDriver();
            
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(MaxLoginAttemptSeconds));

            _pixaiNavigation.NavigateToUrl(driver, StartPageUrl);
            _pixaiNavigation.LogIn(driver, account.Email, account.Password);
            _stringBuilder.AppendLine($"======Account Info======\nEmail : {account.Email}\n Password : {account.Password}");
           
            if (!wait.Until(drv => drv.Url == MainPageUrl))
            {
                _stringBuilder.AppendLine($"Login Status : Failed\n==============================");
                return;
            }


            _pixaiNavigation.NavigateToUrl(driver, UserProfileUrl);

            Thread.Sleep(TimeSpan.FromSeconds(DynamicDataLoadDelay));

            if (settings.ShouldLogEmailVerificationStatus)
            {
                _stringBuilder.AppendLine($"Email Verification Status : {_pixaiDataReader.GetEmailVerificationStatus(driver)}");
            }

            if (settings.ShouldLogAccountId)
            {
                _stringBuilder.AppendLine($"Account Id : {_pixaiDataReader.GetAccountId(driver)}");
            }

            _pixaiNavigation.GoBack(driver);

            while (!driver.Url.Contains('@'))
            {
                _pixaiNavigation.ClickDropdownMenu(driver);
                _pixaiNavigation.NavigateToProfile(driver);
            }

            Thread.Sleep(TimeSpan.FromSeconds(DynamicDataLoadDelay));

            if (settings.ShouldLogAccountUsername)
            {
                _stringBuilder.AppendLine($"Account Id : {_pixaiDataReader.GetUsername(driver)}");
            }

            if (settings.ShouldLogAccountCredits)
            {
                _stringBuilder.AppendLine($"Credits : {_pixaiDataReader.GetCreditsCount(driver)}");

            }

            if (settings.ShouldLogFollowersCount)
            {
                _stringBuilder.AppendLine($"Followers Count : {_pixaiDataReader.GetFollowersCount(driver)}");
            }

            if (settings.ShouldLogFollowingCount)
            {
                _stringBuilder.AppendLine($"Following Count : {_pixaiDataReader.GetFollowingCount(driver)}");
            }


            _stringBuilder.AppendLine("==============================");

            driver.Quit();

        }




        #region Fields

        private readonly IPixaiNavigation _pixaiNavigation;

        private readonly IPixaiDataReader _pixaiDataReader;

        private readonly ILogger _logger;

        private readonly StringBuilder _stringBuilder;

        private const string StartPageUrl = "https://pixai.art/login";

        private const string MainPageUrl = "https://pixai.art/";

        private const string UserProfileUrl = "https://pixai.art/profile/edit";

        private const int DynamicDataLoadDelay = 1;

        private const int MaxLoginAttemptSeconds = 5;


        #endregion
    }
}
