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
using Leaf.xNet;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace PixaiBot.Bussines_Logic.Driver_and_Browser_Management
{
    internal class AccountCreator : IAccountCreator
    {
        private bool _shouldStop;

        public AccountCreator(ITempMailApiManager tempMailApiManager, ILoginCredentialsMaker loginCredentialsMaker, ILogger logger, IProxyManager proxyManager)
        {
            _proxyManager = proxyManager;
            _tempMailApiManager = tempMailApiManager;
            _logger = logger;
            _loginCredentialsMaker = loginCredentialsMaker;
        }

        public event EventHandler<UserAccount>? AccountCreated;

        public event EventHandler<string>? ErrorOccurred;

        private const string StartPageUrl = "https://pixai.art/sign-up";

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
                   driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

                try
                {
                    CreateAccount(driver, shouldVerifyEmail, tempMailApiKey);
                }
                catch (Exception e)
                {
                    _logger.Log("Chrome drive threw exception\n" + e.Message,_logger.CreditClaimerLogFilePath);
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
        /// <summary>
        /// Creates account without email verification 
        /// </summary>
        /// <param name="driver"></param>
        private void CreateAccount(ChromeDriver driver, bool shouldVerifyEmail, string tempMailApiKey)
        {

            _logger.Log("Account creation process started",_logger.CreditClaimerLogFilePath);
            driver.Navigate().GoToUrl(StartPageUrl);

            var email = shouldVerifyEmail ? _loginCredentialsMaker.GenerateEmail(tempMailApiKey) : _loginCredentialsMaker.GenerateEmail();
            var password = _loginCredentialsMaker.GeneratePassword();

            LogInWithEmail(driver);
            EnterEmailAndPassword(driver, email, password);

            ClickSignIn(driver);

            Thread.Sleep(TimeSpan.FromSeconds(2.5));
            
            var isRegisteredSuccessfully = CheckRegistrationSuccess(driver);

            if (!isRegisteredSuccessfully)
            {
                _logger.Log("Too many requests!", _logger.CreditClaimerLogFilePath);
                ErrorOccurred?.Invoke(this, "Too many requests");
                _shouldStop = true;
                return;
            }

            _logger.Log($"Account registered{{ Email:{email}, Password : {password} }} ",_logger.CreditClaimerLogFilePath);


            var createdAccount = new UserAccount
            {
                Email = email,
                Password = password
            };

            AccountCreated?.Invoke(this, createdAccount);

            if (!shouldVerifyEmail) return;

            OpenDropdownMenu(driver);

            ClickProfile(driver);

            ResendVerificationLink(driver);

            VerifyEmail(createdAccount, driver, tempMailApiKey);
        }


        private void LogInWithEmail(ISearchContext driver)
        {
            _logger.Log("Finding button to navigate to registration form", _logger.CreditClaimerLogFilePath);
            IReadOnlyCollection<IWebElement> buttons = driver.FindElements(By.TagName("button"));
            buttons.FirstOrDefault(x => x.Text == "Log in with email")?.Click();
        }


        private void EnterEmailAndPassword(ISearchContext driver, string email, string password)
        {
            _logger.Log("Sending email & password to textboxes ", _logger.CreditClaimerLogFilePath);

            var emailTextBox = driver.FindElement(By.CssSelector("* > * > *:nth-child(2) > * > *:nth-child(1) > * > *"));
            emailTextBox.Click();
            emailTextBox.SendKeys(email);

            var passwordTextBox = driver.FindElement(By.CssSelector("*:nth-child(2) > * > *:nth-child(2) > * > *"));
            passwordTextBox.Click();
            passwordTextBox.SendKeys(password);
        }


        private void ClickSignIn(ISearchContext driver)
        {
            driver.FindElement(By.CssSelector("#\\:r2\\:")).Click();

            _logger.Log("Register button clicked", _logger.CreditClaimerLogFilePath);

        }


        private void ClickProfile(ISearchContext driver)
        {
            _logger.Log("Navigating to account settings", _logger.CreditClaimerLogFilePath);

            var profileButton = driver.FindElement(By.CssSelector(".MuiMenuItem-root:nth-child(3)"));
           
            profileButton?.Click();
        }

        private void OpenDropdownMenu(ISearchContext driver)
        {
            _logger.Log("Trying to find dropdown menu", _logger.CreditClaimerLogFilePath);
            var dropdownMenuButton = driver.FindElement(By.CssSelector(".shrink-0"));
            dropdownMenuButton?.Click();
        }


        private  void ResendVerificationLink(ISearchContext driver)
        {
            var resendVerificationLinkButton = driver.FindElement(By.CssSelector("*:nth-child(3) *:nth-child(2) > *:nth-child(4)"));
            resendVerificationLinkButton.Click();
            _logger.Log("Resend button clicked", _logger.CreditClaimerLogFilePath);
        }


        private  bool CheckRegistrationSuccess(IWebDriver driver)
        {
            _logger.Log("Checking if account registration was successful",_logger.CreditClaimerLogFilePath);
            return driver.Url != StartPageUrl;
        }

        public void VerifyEmail(UserAccount userAccount, ChromeDriver driver, string tempMailApiKey)
        {
            var verificationLink = string.Empty;
            while (string.IsNullOrEmpty(verificationLink))
            {
                verificationLink = _tempMailApiManager.GetVerificationLink(userAccount.Email, tempMailApiKey);
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
            driver.Navigate().GoToUrl(verificationLink);

            Thread.Sleep(TimeSpan.FromSeconds(2.5));

            _logger.Log("Email verified",_logger.ApplicationLogFilePath);
        }

    }
}

