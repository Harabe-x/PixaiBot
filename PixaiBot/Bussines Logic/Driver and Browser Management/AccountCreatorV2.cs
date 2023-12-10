using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;

namespace PixaiBot.Bussines_Logic.Driver_and_Browser_Management
{
    internal class AccountCreatorV2 : IAccountCreator
    {
        public AccountCreatorV2(IPixaiNavigation pixaiNavigation,ITcpServerConnector tcpServerConnector,ITempMailApiManager tempMailApiManager, ILoginCredentialsMaker loginCredentialsMaker, ILogger logger, IProxyManager proxyManager)
        {
            _proxyManager = proxyManager;
            _tempMailApiManager = tempMailApiManager;
            _logger = logger;
            _loginCredentialsMaker = loginCredentialsMaker;
            _tcpServerConnector = tcpServerConnector;
            _pixaiNavigation = pixaiNavigation;
            _tcpServerConnector = tcpServerConnector;
        }

        private bool _shouldStop;

        public event EventHandler<UserAccount>? AccountCreated;

        private readonly ITcpServerConnector _tcpServerConnector;

        private readonly IPixaiNavigation _pixaiNavigation;

        public event EventHandler<string>? ErrorOccurred;

        private const string StartPageUrl = "https://pixai.art/sign-up";

        private const int WaitTime = 1; 

        private readonly IProxyManager _proxyManager;

        private readonly ITempMailApiManager _tempMailApiManager;

        private readonly ILoginCredentialsMaker _loginCredentialsMaker;

        private readonly ILogger _logger;

        /// <summary>
        /// Starts the account creation process
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="tempMailApiKey"></param>
        /// <param name="shouldUseProxy"></param>
        /// <param name="shouldVerifyEmail"></param>
        public void CreateAccounts(int amount, string tempMailApiKey, bool shouldUseProxy, bool shouldVerifyEmail)
        {
            for (var i = 0; i < amount; i++)
            {

                if (_shouldStop) return;

                using var driver = shouldUseProxy ? ChromeDriverFactory.CreateDriver(_proxyManager.GetRandomProxy()) : ChromeDriverFactory.CreateDriverForDebug();
              
                _logger.Log("=====Launched Chrome Driver=====", _logger.CreditClaimerLogFilePath);

                try
                {
                    CreateAccount(driver, shouldVerifyEmail, tempMailApiKey);
                }
                catch (Exception e)
                {
                    _logger.Log("Chrome drive threw exception\n" + e.Message, _logger.CreditClaimerLogFilePath);
                    ErrorOccurred?.Invoke(this, "Chrome drive Exception occurred");
                    _shouldStop = true;
                }
                finally
                {
                    driver.Quit();
                    _logger.Log("=====Chrome Drive Disposed=====\n", _logger.CreditClaimerLogFilePath);

                }
            }
        }

        private void CreateAccount(ChromeDriver driver, bool shouldVerifyEmail, string tempMailApiKey)
        {

            var email = shouldVerifyEmail ? _loginCredentialsMaker.GenerateEmail(tempMailApiKey) : _loginCredentialsMaker.GenerateEmail();
            var password = _loginCredentialsMaker.GeneratePassword();

            _pixaiNavigation.NavigateToUrl(driver, StartPageUrl);
            _pixaiNavigation.GoToLoginPage(driver);
            _pixaiNavigation.SendLoginCredentialsToTextBoxes(driver, email, password);
            _pixaiNavigation.ClickOnRegisterButton(driver);

            Thread.Sleep(TimeSpan.FromSeconds(WaitTime));

            var userAccount = new UserAccount() { Email = email, Password = password };

            AccountCreated?.Invoke(this,userAccount);

            if (!shouldVerifyEmail)
            {
                return;
            }

            while (driver.Url != "https://pixai.art/profile/edit")
            {
                _pixaiNavigation.ClickDropdownMenu(driver);
                _pixaiNavigation.NavigateToProfileSettings(driver);
            }

            _pixaiNavigation.NavigateToProfileSettings(driver);
            // Clicks the Resend button 5 times to ensure that button was clicked 
             _pixaiNavigation.ClickResendEmailVerificationLinkButton(driver);


            VerifyEmail(userAccount,driver,tempMailApiKey);

        }

        private void VerifyEmail(UserAccount userAccount, ChromeDriver driver, string tempMailApiKey)
        {
            var verificationLink = string.Empty;
            while (string.IsNullOrEmpty(verificationLink))
            {
                verificationLink = _tempMailApiManager.GetVerificationLink(userAccount.Email, tempMailApiKey);
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
            driver.Navigate().GoToUrl(verificationLink);
            
            Thread.Sleep(TimeSpan.FromSeconds(2.5));

            _logger.Log("Email verified", _logger.ApplicationLogFilePath);
        }
    }
}
