using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
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
        }


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
        public void CreateAccounts(int amount, string tempMailApiKey, bool shouldUseProxy, bool shouldVerifyEmail,CancellationToken token)
        {
            for (var i = 0; i < amount; i++)
            {

                if (token.IsCancellationRequested)
                {
                    return;
                }

                using var driver = shouldUseProxy ? ChromeDriverFactory.CreateDriver(_proxyManager.GetRandomProxy()) : ChromeDriverFactory.CreateDriver();
              
                _logger.Log("=====Launched Chrome Driver=====", _logger.CreditClaimerLogFilePath);

                try
                {
                    CreateAccount(driver, shouldVerifyEmail, tempMailApiKey);
                }
                catch (Exception e)
                {
                    _tcpServerConnector.SendMessage("rChrome drive threw exception\n" + e.Message);
                    _logger.Log("Chrome drive threw exception\n" + e.Message, _logger.CreditClaimerLogFilePath);
                    ErrorOccurred?.Invoke(this, "Chrome drive Exception occurred");
                   continue;

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


            //In some cases, after pressing the button to go to account model, the user remains on the home page, so the code is executed until the url is correct.
            while (driver.Url != "https://pixai.art/profile/edit")
            {
                _pixaiNavigation.ClickDropdownMenu(driver);
                _pixaiNavigation.NavigateToProfileSettings(driver);
            }

            _pixaiNavigation.NavigateToProfileSettings(driver); 
            _pixaiNavigation.ClickResendEmailVerificationLinkButton(driver);

          

            VerifyEmail(userAccount,driver,tempMailApiKey);

        }

        private void VerifyEmail(UserAccount userAccount, ChromeDriver driver, string tempMailApiKey)
        {
            var verificationLink = string.Empty;
            const int maxAttempts = 10; 
            var attemptCount = 0;

            while (string.IsNullOrEmpty(verificationLink) && attemptCount < maxAttempts)
            {
                verificationLink = _tempMailApiManager.GetVerificationLink(userAccount.Email, tempMailApiKey);
                if (!string.IsNullOrEmpty(verificationLink)) continue;
                Thread.Sleep(TimeSpan.FromSeconds(5)); 
                attemptCount++;
            }

            if (string.IsNullOrEmpty(verificationLink))
            {
                _logger.Log("Email verification link not found", _logger.ApplicationLogFilePath);
                ErrorOccurred?.Invoke(this, "Email verification link not found after maximum attempts.");
                _tcpServerConnector.SendMessage("rEmail verification link not found after maximum attempts.");
                return;
            }
           
            driver.Navigate().GoToUrl(verificationLink);
            
            Thread.Sleep(TimeSpan.FromSeconds(2.5));

            _logger.Log("Email verified", _logger.ApplicationLogFilePath);
        }
    }
}
