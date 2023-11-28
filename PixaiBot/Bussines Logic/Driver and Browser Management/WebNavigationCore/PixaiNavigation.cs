using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PixaiBot.Bussines_Logic.Driver_and_Browser_Management.WebNavigationCore.WebNavigationCoreException;
using PixaiBot.Data.Interfaces;

namespace PixaiBot.Bussines_Logic.Driver_and_Browser_Management.WebNavigationCore
{
    internal class PixaiNavigation : IPixaiNavigation
    {
        private const int PageLoadWaitTime = 1000;

        public PixaiNavigation(ILogger logger)
        {
            _logger = logger;
        }

        private readonly ILogger _logger;

        public void GoBack(ChromeDriver driver)
        {
            driver.Navigate().Back();
        }

        public void GoToRegistrationPage(ChromeDriver driver)
        {
            _logger.Log("Finding button to navigate to registration form", _logger.CreditClaimerLogFilePath);

            try
            {
                IReadOnlyCollection<IWebElement> buttons = driver.FindElements(By.TagName("button"));
                buttons.FirstOrDefault(x => x.Text == "Log in with email")?.Click();
            }
            catch (Exception e)
            {
                throw new ChromeDriverException("ChromeDriver exception occurred", e);
            }
        }

        public void GoToLoginPage(ChromeDriver driver)
        {
            _logger.Log("Finding button to navigate to registration form", _logger.CreditClaimerLogFilePath);

            try
            {
                IReadOnlyCollection<IWebElement> buttons = driver.FindElements(By.TagName("button"));
                buttons.FirstOrDefault(x => x.Text == "Log in with email")?.Click();
            }
            catch (Exception e)
            {
                throw new ChromeDriverException("ChromeDriver exception occurred", e);
            }
        }

        public void SendCredentialsToTextBoxes(ChromeDriver driver, string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                throw new ArgumentException("login credentials can't be null");

            _logger.Log("Sending email & password to textboxes ", _logger.CreditClaimerLogFilePath);
            try
            {
                var emailTextBox = driver.FindElement(By.CssSelector("* > * > *:nth-child(2) > * > *:nth-child(1) > * > *"));
                emailTextBox.Click();
                emailTextBox.SendKeys(email);

                var passwordTextBox = driver.FindElement(By.CssSelector("*:nth-child(2) > * > *:nth-child(2) > * > *"));
                passwordTextBox.Click();
                passwordTextBox.SendKeys(password);
            }
            catch (Exception e)
            {
                throw new ChromeDriverException("ChromeDriver exception occurred", e);
            }

        }

        public void ClickOnRegisterButton(ChromeDriver driver)
        {
            try
            {
                driver.FindElement(By.CssSelector("#\\:r2\\:")).Click();
            }
            catch (Exception e)
            {
                throw new ChromeDriverException("ChromeDriver exception occurred", e);
            }

            Thread.Sleep(TimeSpan.FromMilliseconds(PageLoadWaitTime));

            _logger.Log("Login button clicked", _logger.CreditClaimerLogFilePath);
        }

        public void ClickOnLoginButton(ChromeDriver driver)
        {
            try
            {
                driver.FindElement(By.CssSelector("#\\:r2\\:")).Click();
            }
            catch (Exception e)
            {
                throw new ChromeDriverException("ChromeDriver exception occurred", e);
            }

            Thread.Sleep(TimeSpan.FromMilliseconds(PageLoadWaitTime));

            _logger.Log("Login button clicked", _logger.CreditClaimerLogFilePath);
        }

        public void GoToProfile(ChromeDriver driver)
        {
            try
            {
                var profileButton = driver.FindElement(By.CssSelector(".MuiMenuItem-root:nth-child(1)"));
                profileButton?.Click();
            }
            catch (Exception e)
            {
                throw new ChromeDriverException("ChromeDriver exception occurred", e);
            }
        }

        public void GoToProfileSettings(ChromeDriver driver)
        {
            _logger.Log("Navigating to account settings", _logger.CreditClaimerLogFilePath);

            try
            {
                var profileButton = driver.FindElement(By.CssSelector(".MuiMenuItem-root:nth-child(3)"));
                profileButton?.Click();
            }
            catch (Exception e)
            {
                throw new ChromeDriverException("ChromeDriver exception occurred", e);
            }
        }

        public void GoToCreditsTab(ChromeDriver driver)
        {
            if (driver.Url is "https://pixai.art") throw new InvalidPageContentException("Url should not be the url of the home page");

            NavigateToUrl(driver, driver.Url + "/credits");
        }

        public void ClickDropdownMenu(ChromeDriver driver)
        {

            if (driver.Url is "https://pixai.art/login" or "https://pixai.art/sign-up") throw new InvalidPageContentException("Url should not be the url of the login page or registration page");

            _logger.Log("Dropdown Menu Opened", _logger.CreditClaimerLogFilePath);
            try
            {
                var dropdownMenuButton = driver.FindElement(By.CssSelector(".shrink-0"));
                dropdownMenuButton?.Click();
            }
            catch (Exception e)
            {
                throw new ChromeDriverException("ChromeDriver exception occurred", e);
            }

        }

        public void GoToMyWorkTab(ChromeDriver driver)
        {
            if (driver.Url is "https://pixai.art") throw new InvalidPageContentException("Url should not be the url of the home page");
            NavigateToUrl(driver, driver.Url + "/artwork");
        }

        public void NavigateToUrl(ChromeDriver driver, string url)
        {
            driver.Navigate().GoToUrl(url);

            Thread.Sleep(TimeSpan.FromMilliseconds(PageLoadWaitTime));
        }
    }
}
