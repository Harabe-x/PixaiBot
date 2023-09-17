using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;

namespace PixaiBot.Bussines_Logic
{
    internal class CreditClaimer : LoginModule, ICreditClaimer
    {
        private ChromeDriver? _driver;

        private const int WaitTime = 1500;

        private const int DelayBetweenClicks = 100; 

        private const string LoginUrl = "https://pixai.art/login/";

        private readonly ILogger _logger;

        public CreditClaimer(ILogger logger)
        {
            _logger = logger;
        }

        public void ClaimCredits(UserAccount account)
        {
            _driver = new ChromeDriver();

            LoginModule.Login(_driver, account);

            if (_driver.Url == LoginUrl)
            {
                _logger.Log("Login failed");
                _driver.Quit();
                return;
            }

            _logger.Log("Logged in successfully");

            if (!GoToProfile())
            {
                _driver.Quit();
                return;
            }
            _logger.Log("Navigated to profile page");
            if (!ClaimCreditsOnAccount())
            {
                _driver.Quit();
                return;
            }
            _driver.Quit();
            
            _logger.Log($"Claiming credits completed successfully for {account.Email}");

        }

        private bool GoToProfile()
        {
            Thread.Sleep(WaitTime);

            try
            {
                var dropdownMenuButton = _driver.FindElement(By.CssSelector(".cursor-pointer:nth-child(7)"));
                dropdownMenuButton?.Click();
               
                Thread.Sleep(DelayBetweenClicks);
              
                var profileButton = _driver.FindElement(By.CssSelector(".MuiMenuItem-root:nth-child(1)"));
                profileButton.Click();

                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private bool ClaimCreditsOnAccount()
        {
            Thread.Sleep(WaitTime);

            try
            {
                var creditsTab = _driver.FindElement(By.CssSelector(".sc-eYqcxL:nth-child(5)"));
                creditsTab?.Click();

                Thread.Sleep(WaitTime);

                IReadOnlyCollection<IWebElement> buttons = _driver.FindElements(By.TagName("button"));

                buttons.FirstOrDefault(x => x.Text == "Claim them!")?.Click();
                
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }


    }
}
