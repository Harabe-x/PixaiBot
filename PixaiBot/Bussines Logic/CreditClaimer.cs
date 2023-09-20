﻿using System;
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

        private const int Delay = 50;

        private const string LoginUrl = "https://pixai.art/login/";

        private readonly ILogger _logger;


        public CreditClaimer(ILogger logger)
        {
            _logger = logger;
        }

        public void ClaimCredits(UserAccount account, IToastNotificationSender toastNotificationSender = null)
        {
      
            _driver = new ChromeDriver();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(MaxWaitTime);
            LoginModule.Login(_driver, account);
            if (_driver.Url == LoginUrl)
            {
                toastNotificationSender?.SendNotification("Login failed", $"Login failed for {account.Email}", NotificationType.Error);
                _logger.Log("Login failed");
                _driver.Quit();
                return ;
            }

            _logger.Log("Logged in successfully");

            if (!GoToProfile())
            {
                toastNotificationSender?.SendNotification("Claiming process failed", $"if this error persists, please open new issue on github ", NotificationType.Error);
                _logger.Log("Going to profile failed");
                _driver.Quit();
                return;
            }
            _logger.Log("Navigated to profile page");
            if (!ClaimCreditsOnAccount())
            {
              toastNotificationSender?.SendNotification("Claiming claimed", $"Try again tomorrow", NotificationType.Warning); ;
                _driver.Quit();
                return;
            }   
            toastNotificationSender?.SendNotification("Claiming credits completed successfully", $"Claiming credits completed successfully for {account.Email}", NotificationType.Success);
            _logger.Log($"Claiming credits completed successfully for {account.Email}");
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

        private bool ClaimCreditsOnAccount()
        {
            try
            {

                _logger.Log("Trying to find credits tab ...");
                var creditsTab = _driver.FindElement(By.CssSelector(".sc-jSUZER:nth-child(5)"));
                creditsTab?.Click();
                _logger.Log("Finding buttons ...");
                Thread.Sleep(500);
                var claimButton = _driver.FindElement(By.CssSelector(".MuiLoadingButton-root"));


                for (var i = 0; i < 5; i++)
                {
                    Thread.Sleep(Delay);
                    claimButton.Click();
                }
                return true;
            }
            catch (NoSuchElementException)
            {
                _logger.Log("Element not found, canceling claiming process");
                return false;
            }
            catch (StaleElementReferenceException)
            {
                Thread.Sleep(Delay);
                ClaimCreditsOnAccount();
            }
            catch (ElementClickInterceptedException)
            {
                Thread.Sleep(Delay);
            }
            return false;
        }
    }
}
