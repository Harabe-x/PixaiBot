using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Linq;
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

public class AccountLoginChecker : IAccountLoginChecker
{
    private ChromeDriver? _driver;

    private const string MainPageUrl = "https://pixai.art/";

    private readonly string _accountsFilePath;

    private readonly ILogger _logger;

    public AccountLoginChecker(ILogger logger)
    {
        _accountsFilePath = InitialConfiguration.AccountsFilePath;
        _logger = logger;

    }
    /// <summary>
    /// Checks account login credentials 
    /// </summary>
    /// <param name="userAccount"></param>
    /// <param name="toastNotificationSender"></param>
    /// <returns>True if the account is valid</returns>
    public bool CheckAccountLogin(UserAccount userAccount,IToastNotificationSender toastNotificationSender)
    {
        _driver = ChromeDriverFactory.CreateDriver();

        LoginModule.Login(_driver, userAccount, _logger);

        Thread.Sleep(1000);

        if (_driver.Url == MainPageUrl)
        {
            _driver.Close();
            _driver.Dispose();
            _logger.Log($"Valid Account {userAccount.Email}", _logger.CreditClaimerLogFilePath);
            _logger.Log($"=====Chrome Drive Disposed=====\n", _logger.CreditClaimerLogFilePath);
            toastNotificationSender?.SendNotification("PixaiBot", $"Valid Account {userAccount.Email}",NotificationType.Success);
            return true;
        }

        _driver.Close();
        _driver.Dispose();
        _logger.Log("Invalid Account", _logger.CreditClaimerLogFilePath);
        _logger.Log("=====Chrome Drive Disposed=====\n", _logger.CreditClaimerLogFilePath);
        toastNotificationSender?.SendNotification("PixaiBot", $"Invalid Account {userAccount.Email}",NotificationType.Error);
        return false;
    }
    /// <summary>
    /// Checks all accounts login credentials
    /// </summary>
    /// <param name="accountsList"></param>
    /// <param name="toastNotificationSender"></param>
    /// <returns>Number of valid accounts</returns>
    public int CheckAllAccountsLogin(IList<UserAccount> accountsList,IToastNotificationSender toastNotificationSender = null)
    {
        var validAccounts = accountsList.Where((account) => CheckAccountLogin(account,toastNotificationSender)).ToList();

        JsonWriter.WriteJson(validAccounts, _accountsFilePath);
        
        return validAccounts.Count;
    }
}