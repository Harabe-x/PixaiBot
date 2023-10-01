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

namespace PixaiBot.Bussines_Logic;

public class CreditClaimer : ICreditClaimer
{
    private ChromeDriver? _driver;

    private const int MaxWaitTime = 5;

    private const int Delay = 50;

    private const string LoginUrl = "https://pixai.art/login/";

    private readonly ILogger _logger;

    public event EventHandler<UserAccount> CreditClaimed;


    public CreditClaimer(ILogger logger)
    {
        _logger = logger;
    }


    public void ClaimCreditsForAllAccounts(IEnumerable<UserAccount> accounts, IToastNotificationSender toastNotificationSender = null)
    {
        foreach (var account in accounts)
        {
            CreditClaimed?.Invoke(this,account);
            ClaimCredits(account,toastNotificationSender);
        }
    }


    /// <summary>
    /// Claims credits on the given account
    /// </summary>
    /// <param name="account"></param>
    /// <param name="toastNotificationSender"></param>
    public void ClaimCredits(UserAccount account, IToastNotificationSender toastNotificationSender)
    {
        _driver = ChromeDriverFactory.CreateDriver();

        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(MaxWaitTime);

        _driver.Manage().Window.Minimize();
        LoginModule.Login(_driver, account, _logger);
        if (_driver.Url == LoginUrl)
        {
            toastNotificationSender?.SendNotification("PixaiBot", $"Login failed for {account.Email}",
                NotificationType.Error);
            _logger.Log("Login failed", _logger.CreditClaimerLogFilePath);
            _driver.Quit();
            _logger.Log("=====Chrome Drive Disposed=====\n", _logger.CreditClaimerLogFilePath);
            return;
        }

        _logger.Log("Logged in successfully", _logger.CreditClaimerLogFilePath);

        if (!GoToProfile())
        {
            toastNotificationSender?.SendNotification("PixaiBot",
                $"Claiming process failed,if this error persists, please open new issue on github ", NotificationType.Error);
            _logger.Log("Going to profile failed", _logger.CreditClaimerLogFilePath);
            _driver.Quit();
            _logger.Log("=====Chrome Drive Disposed=====\n", _logger.CreditClaimerLogFilePath);
            return;
        }

        _logger.Log("Navigated to profile page", _logger.CreditClaimerLogFilePath);
        if (!ClaimCreditsOnAccount())
        {
            toastNotificationSender?.SendNotification("PixaiBot", $"Credits claimed,Try again tomorrow",
                NotificationType.Warning);
            _driver.Quit();
            _logger.Log("=====Chrome Drive Disposed=====\n", _logger.CreditClaimerLogFilePath);
            return;
        }

        toastNotificationSender?.SendNotification("PixaiBot",
            $"Claiming credits completed successfully for {account.Email}", NotificationType.Success);
        _logger.Log($"Claiming credits completed successfully for {account.Email}", _logger.CreditClaimerLogFilePath);
        _driver.Quit();
        _logger.Log("=====Chrome Drive Disposed=====\n", _logger.CreditClaimerLogFilePath);
    }

    /// <summary>
    ///  Navigates to profile page
    /// </summary>
    /// <returns> Returns true if the operation is successful; otherwise false.</returns>
    private bool GoToProfile()
    {
        try
        {
            _logger.Log("Trying to find dropdown menu button ...", _logger.CreditClaimerLogFilePath);
            var dropdownMenuButton = _driver.FindElement(By.CssSelector(".shrink-0"));
            dropdownMenuButton?.Click();


            _logger.Log("Trying to find profile tab ...", _logger.CreditClaimerLogFilePath);
            var profileButton = _driver.FindElement(By.CssSelector(".MuiMenuItem-root:nth-child(1)"));
            profileButton.Click();

            return true;
        }
        catch (NoSuchElementException)
        {
            _logger.Log("Element not found, canceling claiming process", _logger.CreditClaimerLogFilePath);
            return false;
        }
    }

    /// <summary>
    ///  Claims credits on the account
    /// </summary>
    /// <returns>Returns true if the operation is successful; otherwise false.</returns>
    private bool ClaimCreditsOnAccount()
    {
        try
        {
            _logger.Log("Trying to find credits tab ...", _logger.CreditClaimerLogFilePath);
            var creditsTab = _driver.FindElement(By.CssSelector(".sc-jSUZER:nth-child(5)"));
            creditsTab?.Click();
            _logger.Log("Finding buttons ...", _logger.CreditClaimerLogFilePath);
            Thread.Sleep(500);
            var claimButton = _driver.FindElement(By.CssSelector(".MuiLoadingButton-root"));

            // Clicks the claim button 5 times to ensure that button was clicked 
            for (var i = 0; i < 5; i++)
            {
                Thread.Sleep(Delay);
                claimButton.Click();
            }

            return true;
        }
        catch (NoSuchElementException)
        {
            _logger.Log("Element not found, canceling claiming process", _logger.CreditClaimerLogFilePath);
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

        return true;
    }
}