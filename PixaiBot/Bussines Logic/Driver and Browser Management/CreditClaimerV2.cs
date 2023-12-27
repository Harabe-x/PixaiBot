using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Notification.Wpf;
using PixaiBot.Bussines_Logic.Driver_and_Browser_Management.WebNavigationCore.WebNavigationCoreException;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;

namespace PixaiBot.Bussines_Logic.Driver_and_Browser_Management
{
    internal class CreditClaimerV2 : ICreditClaimer
    {
        private const string LoginUrl = "https://pixai.art/login";

        public event EventHandler<UserAccount>? CreditsClaimed;

        public event EventHandler<UserAccount>? ProcessStartedForAccount;

        public CreditClaimerV2(ILogger logger, IPixaiNavigation pixaiNavigation,ITcpServerConnector tcpServerConnector)
        {
            _logger = logger;

            _tcpServerConnector = tcpServerConnector;

            _pixaiNavigation = pixaiNavigation;
        }
           
        private readonly ILogger _logger;

        private readonly ITcpServerConnector _tcpServerConnector;

        private readonly IPixaiNavigation _pixaiNavigation;

        public void ClaimCredits(UserAccount account)
        {


            using var driver = ChromeDriverFactory.CreateDriver();

            _logger.Log("=====Launched Chrome Driver=====",_logger.CreditClaimerLogFilePath);

            _tcpServerConnector.SendMessage("gLaunched Chrome Driver");

            try  
            {
                _pixaiNavigation.NavigateToUrl(driver, LoginUrl);
                _pixaiNavigation.GoToLoginPage(driver);
                _pixaiNavigation.SendLoginCredentialsToTextBoxes(driver,account.Email,account.Password);
                _pixaiNavigation.ClickOnLoginButton(driver);
               

                //Ensures that user in on profile page
                while (!driver.Url.Contains('@'))
                {
                    _pixaiNavigation.ClickDropdownMenu(driver);
                    _pixaiNavigation.NavigateToProfile(driver);
                    _pixaiNavigation.GoToCreditsTab(driver);

                }
                // Clicks the claim button 5 times to ensure that button was clicked 
                for (var i =  0; i < 5; i++) _pixaiNavigation.ClickClaimCreditButton(driver);
                _tcpServerConnector.SendMessage("gClaimed credits");
            }
            catch (ChromeDriverException e)
            {
                _logger.Log(e.Message, _logger.CreditClaimerLogFilePath);
            }
            catch (InvalidPageContentException e)
            {
                _logger.Log(e.Message, _logger.CreditClaimerLogFilePath);
            }
            _tcpServerConnector.SendMessage("gClaimed credits");


            driver.Quit();

            _logger.Log("=====Chrome Drive Closed=====\n",_logger.ApplicationLogFilePath);
        }

        public void ClaimCreditsForAllAccounts(IEnumerable<UserAccount> accounts, CancellationToken cancellationToken)
        {

            _tcpServerConnector.SendMessage($"cClaiming credits on {accounts.Count()} accounts");

            foreach (var account in accounts)
            {
                if (cancellationToken.IsCancellationRequested) return;

                ProcessStartedForAccount?.Invoke(this,account);

                ClaimCredits(account);

                CreditsClaimed?.Invoke(this, account);

            }
        }
    }
}
