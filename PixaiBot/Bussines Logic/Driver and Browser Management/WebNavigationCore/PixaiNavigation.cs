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
        private const string LoginPageUrl = "https://pixai.art/login";

        private const string RegistrationPageUrl = "https://pixai.art/sign-up";

        private const string HomePageUrl = "https://pixai.art/";

        private const int PageLoadWaitTime = 1000;

        public PixaiNavigation(ILogger logger,ITcpServerConnector tcpServerConnector)
        {
            _tcpServerConnector = tcpServerConnector;
            _logger = logger;
        }

        private readonly ILogger _logger;

        private readonly ITcpServerConnector _tcpServerConnector;

        public void ClickResendEmailVerificationLinkButton(ISearchContext searchContext)
        {
            _tcpServerConnector.SendMessage("yFinding button to resend verification link");

            ClickElement(searchContext, "*:nth-child(3) *:nth-child(2) > *:nth-child(4)");
        }

        public void GoBack(IWebDriver driver)
        {
            driver.Navigate().Back();
        }

        public void NavigateRegistrationPage(ISearchContext driver)
        {
            _logger.Log("Finding button to navigate to registration form", _logger.CreditClaimerLogFilePath);
            _tcpServerConnector.SendMessage("yFinding button to navigate to registration form");
            ClickElementWithSpecifiedText(driver, "button", "Register");
        }

        public void GoToLoginPage(ISearchContext driver)
        {
            _logger.Log("Finding button to navigate to registration form", _logger.CreditClaimerLogFilePath);
            _tcpServerConnector.SendMessage("yFinding button to navigate to registration form");
            ClickElementWithSpecifiedText(driver,"button", "Log in with email");
        }

        public void SendLoginCredentialsToTextBoxes(ISearchContext driver, string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                throw new ArgumentException("Login credentials can't be null or empty");

            _logger.Log("Sending email & password to textboxes ", _logger.CreditClaimerLogFilePath);

            _tcpServerConnector.SendMessage("ySending email & password to textboxes");

            SendKeysToElement(driver, "* > * > *:nth-child(2) > * > *:nth-child(1) > * > *",email);

            SendKeysToElement(driver, "*:nth-child(2) > * > *:nth-child(2) > * > *",password);
        }

        public void ClickOnRegisterButton(ISearchContext driver)
        {
            _logger.Log("Clicking register button", _logger.CreditClaimerLogFilePath);

            _tcpServerConnector.SendMessage("yClicking register button");

            ClickElementWithSpecifiedText(driver, "button", "Sign Up");

        }

        public void ClickOnLoginButton(ISearchContext driver)
        {
            _logger.Log("Clicking login button ", _logger.CreditClaimerLogFilePath);

            _tcpServerConnector.SendMessage("yClicking login button");

            ClickElementWithSpecifiedText(driver,"button","Login");

        }

        public void NavigateToProfile(ISearchContext driver)
        {
            _logger.Log("Navigating to profile", _logger.CreditClaimerLogFilePath);

            _tcpServerConnector.SendMessage("yNavigating to profile");
         
            ClickElement(driver, ".MuiMenuItem-root:nth-child(1)");
        }

        public void NavigateToProfileSettings(ISearchContext driver)
        {
            _logger.Log("Navigating to account settings", _logger.CreditClaimerLogFilePath);

            _tcpServerConnector.SendMessage("yNavigating to account settings");
            
            ClickElement(driver,".MuiMenuItem-root:nth-child(3)");
        }

        public void GoToCreditsTab(IWebDriver driver)
        {
            NavigateToUrl(driver, driver.Url + "/credits");
        }

        public void ClickDropdownMenu(IWebDriver driver)
        {
            _tcpServerConnector.SendMessage("yClicking dropdown menu");
            ClickElement(driver, ".shrink-0");
        }
        public void NavigateToMyWorkTab(IWebDriver driver)
        {
            _tcpServerConnector.SendMessage("yNavigating to my work tab");
            NavigateToUrl(driver, driver.Url + "/artwork");
        }

        public void NavigateToUrl(IWebDriver driver, string url)
        {
            driver.Navigate().GoToUrl(url);
            _tcpServerConnector.SendMessage($"yNavigating to {url}");

            Thread.Sleep(TimeSpan.FromMilliseconds(PageLoadWaitTime));
        }

        public void ClickClaimCreditButton(ISearchContext searchContext)
        {
            try
            {
                ClickElement(searchContext, ".MuiLoadingButton-root");
            }
            catch (ElementClickInterceptedException e)
            {
                ClickClaimCreditButton(searchContext);
            }
          
        }


        private static void ClickElement(ISearchContext driver, string cssSelector)
        {
            try
            {
                driver.FindElement(By.CssSelector(cssSelector)).Click();
            }
            catch (Exception e)
            {
                throw new ChromeDriverException("ChromeDriver exception occurred", e);
            }
        }

        private static void ClickElementWithSpecifiedText(ISearchContext driver, string tagName, string text)
        {
            try
            {
                IReadOnlyCollection<IWebElement> buttons = driver.FindElements(By.TagName(tagName));
                buttons.FirstOrDefault(x => x.Text == text )?.Click();
            }
            catch (Exception e)
            {
                throw new ChromeDriverException("ChromeDriver exception occurred", e);
            }
        }

        private static void SendKeysToElement(ISearchContext driver, string cssSelector, string keys)
        {
            try
            {
               var element = driver.FindElement(By.CssSelector(cssSelector));
               element.Click();
               element.SendKeys(keys);
            }
            catch (Exception e)
            {
                throw new ChromeDriverException("ChromeDriver exception occurred", e);
            }
        }

    }
}
