using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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

    public bool CheckAccountLogin(UserAccount userAccount)
    {
        _driver = ChromeDriverFactory.CreateDriver();

        LoginModule.Login(_driver, userAccount, _logger);

        Thread.Sleep(1000);

        if (_driver.Url == MainPageUrl)
        {
            _driver.Close();
            _driver.Dispose();
            _logger.Log($"Valid Account {userAccount.Email}",_logger.CreditClaimerLogFilePath);
           _logger.Log($"=====Chrome Drive Disposed=====\n",_logger.CreditClaimerLogFilePath);
            return true;
        }

        _driver.Close();
        _driver.Dispose();
            _logger.Log("Invalid Account",_logger.CreditClaimerLogFilePath);
            _logger.Log("=====Chrome Drive Disposed=====\n", _logger.CreditClaimerLogFilePath);
            return false;
    }

    public int CheckAllAccountsLogin(IList<UserAccount> accountsList)
    {
        var validAccounts = accountsList.Where(CheckAccountLogin).ToList();

        JsonWriter.WriteJson(validAccounts, _accountsFilePath);

        return validAccounts.Count;
    }
}