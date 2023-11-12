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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace PixaiBot.Bussines_Logic.Driver_and_Browser_Management
{
    internal class AccountCreator : IAccountCreator
    {
        private bool _shouldStop;

        private const int WaitTime = 1;

        public AccountCreator(ITempMailApiManager tempMailApiManager, ILogger logger, IProxyManager proxyManager)
        {
            _proxyManager = proxyManager;
            _tempMailApiManager = tempMailApiManager;
            _logger = logger;


        }

        public event EventHandler<UserAccount>? AccountCreated;

        public event EventHandler<string>? ErrorOccurred;

        private const string StartPageUrl = "https://pixai.art/login";

        private readonly IProxyManager _proxyManager;

        private readonly ITempMailApiManager _tempMailApiManager;

        private readonly ILogger _logger;

        public void CreateAccounts(int amount, string tempMailApiKey, bool shouldUseProxy, bool shouldVerifyEmail)
        {
            ChromeDriver driver;

            for (var i = 0; i < amount; i++)
            {

                if(_shouldStop) return;

                driver = shouldUseProxy ? ChromeDriverFactory.CreateDriver(_proxyManager.GetRandomProxy()) : ChromeDriverFactory.CreateDriverForDebug();

                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

                if (shouldVerifyEmail) CreateAccount(driver, tempMailApiKey);
                else CreateAccount(driver);

            }
        }

        private void CreateAccount(ChromeDriver driver)
        {

            driver.Navigate().GoToUrl(StartPageUrl);


            var email = MakeEmail();

            var password = MakePassword();

            IReadOnlyCollection<IWebElement> buttons = driver.FindElements(By.TagName("button"));

            buttons.FirstOrDefault(x => x.Text == "Log in with email")?.Click();

            driver.FindElement(By.CssSelector("*:nth-child(1) > *:nth-child(4) > *:nth-child(1)")).Click();

            var emailTextBox = driver.FindElement(By.CssSelector("* > * > *:nth-child(2) > * > *:nth-child(1) > * > *"));
            emailTextBox.Click();
            emailTextBox.SendKeys(email);

            var passwordTextBox = driver.FindElement(By.CssSelector("*:nth-child(2) > * > *:nth-child(2) > * > *"));
            passwordTextBox.Click();
            passwordTextBox.SendKeys(password);

            driver.FindElement(By.CssSelector("#\\:r2\\:")).Click();

            var createdAccount = new UserAccount()
            {
                Email = email,
                Password = password
            };
            AccountCreated?.Invoke(this, createdAccount);
        }

        private void CreateAccount(ChromeDriver driver, string tempMailApiKey)
        {
            driver.Navigate().GoToUrl(StartPageUrl);

            Thread.Sleep(TimeSpan.FromSeconds(WaitTime));

            var email = _tempMailApiManager.GetEmail(tempMailApiKey);
        
            if (string.IsNullOrEmpty(email))
            {
                _logger.Log("Invalid ApiKey",
                _logger.CreditClaimerLogFilePath);
                ErrorOccurred?.Invoke(this, "Invalid api key");
                _shouldStop = true;
                return;
            }
            var password = MakePassword();

            IReadOnlyCollection<IWebElement> buttons = driver.FindElements(By.TagName("button"));

            buttons.FirstOrDefault(x => x.Text == "Log in with email")?.Click();

            driver.FindElement(By.CssSelector("*:nth-child(1) > *:nth-child(4) > *:nth-child(1)")).Click();

            var emailTextBox = driver.FindElement(By.CssSelector("* > * > *:nth-child(2) > * > *:nth-child(1) > * > *"));
            emailTextBox.Click();
            emailTextBox.SendKeys(email);

            var passwordTextBox = driver.FindElement(By.CssSelector("*:nth-child(2) > * > *:nth-child(2) > * > *"));
            passwordTextBox.Click();
            passwordTextBox.SendKeys(password);

            driver.FindElement(By.CssSelector("#\\:r2\\:")).Click();

            Thread.Sleep(TimeSpan.FromSeconds(10));

            var verificationLink = _tempMailApiManager.GetVerificationLink(email, tempMailApiKey);

            driver.Navigate().GoToUrl(verificationLink);

            Thread.Sleep(TimeSpan.FromMilliseconds(300));

            var createdAccount = new UserAccount()
            {
                Email = email,
                Password = password
            };
            AccountCreated?.Invoke(this,createdAccount);
        }


        private static string MakeEmail()
        {
            const string letters = "abcdefghijklmnopqrstuvwxyz";
            var random = new Random();
            var firstEmailPart =
                new string(Enumerable.Repeat(letters, 6).Select(x => x[random.Next(letters.Length)]).ToArray());
            return firstEmailPart + "@gmail.com";
        }

        private static string MakePassword()
        {
            const string characters = "abcd[{efghij@#$klmnopqrstu}]vw.><xyz!%^&*";
            var random = new Random();
            return new string(Enumerable.Repeat(characters, 8).Select(x => x[random.Next(characters.Length)])
                .ToArray());
        }



    }
}
