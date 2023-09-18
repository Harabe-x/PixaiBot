using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Notification.Wpf;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;

namespace PixaiBot.Bussines_Logic
{
    internal class CreditClaimer : LoginModule, ICreditClaimer
    {
        private ChromeDriver? _driver;

        private const int MaxWaitTime = 5;

        private const string LoginUrl = "https://pixai.art/login/";

        private readonly ILogger _logger;

        public CreditClaimer(ILogger logger)
        {
            _logger = logger;
        }

        public void ClaimCredits(UserAccount account, IToastNotificationSender notificationSender = null)
        {
            _driver = new ChromeDriver();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(MaxWaitTime);
            LoginModule.Login(_driver, account);
            if (_driver.Url == LoginUrl)
            {
                notificationSender?.SendNotification("Login Failed", $"Failed login for : {account.Email}, check your credentials", NotificationType.Error);
                _logger.Log("Login failed");
                _driver.Quit();
                return;
            }

            _logger.Log("Logged in successfully");

            if (!GoToProfile())
            {

                _logger.Log("Going to profile failed");
                _driver.Quit();
                return;
            }
            _logger.Log("Navigated to profile page");
            if (!ClaimCreditsOnAccount(notificationSender))
            {

                _driver.Quit();
                return;
            }
            _logger.Log($"Claiming credits completed successfully for {account.Email}");
            notificationSender?.SendNotification("Credits Claimed", $"Claiming credits completed successfully for {account.Email}", NotificationType.Success);
            _driver.Quit();

        }

        private bool GoToProfile()
        {

            try
            {
                _logger.Log("Trying to find dropdown menu button ...");
                var dropdownMenuButton = _driver.FindElement(By.CssSelector(".flex-shrink-0:nth-child(8)"));
                dropdownMenuButton?.Click();


                _logger.Log("Trying to find profile tab ...");
                var profileButton = _driver.FindElement(By.CssSelector(".MuiMenuItem-root:nth-child(1)"));
                profileButton.Click();

                return true;
            }
            catch (NoSuchElementException)
            {
                _logger.Log("Element not found, canceling claiming process");
                return false;
            }
        }

        private bool ClaimCreditsOnAccount(IToastNotificationSender notificationSender)
        {
            try
            {

                _logger.Log("Trying to find credits tab ...");
                var creditsTab = _driver.FindElement(By.CssSelector(".sc-jSUZER:nth-child(5)"));
                creditsTab?.Click();

                IReadOnlyCollection<IWebElement> buttons = _driver.FindElements(By.TagName("button"));

                _logger.Log("Finding buttons ...");

                Thread.Sleep(250);
                var claimButton = buttons.FirstOrDefault(x => x.Text == "Claim them!");

                if (claimButton == null)
                {
                    notificationSender?.SendNotification("Credits Already CLaimed", "You already claimed toady !",
                        NotificationType.Warning);
                    _logger.Log("Credits already claimed");
                    return false;
                }

                claimButton?.Click();

                return true;
            }
            catch (NoSuchElementException)
            {
                _logger.Log("Element not found, canceling claiming process");
                return false;
            }
            catch (StaleElementReferenceException)
            {
                Thread.Sleep(50);
                ClaimCreditsOnAccount(notificationSender);
            }
            return false;
        }
    }
}
