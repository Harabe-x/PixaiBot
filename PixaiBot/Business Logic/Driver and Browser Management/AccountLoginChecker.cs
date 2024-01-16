using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using PixaiBot.Business_Logic.Driver_and_Browser_Management.Driver_Creation_Strategy;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Models;

namespace PixaiBot.Business_Logic.Driver_and_Browser_Management;

public class AccountLoginChecker : IAccountLoginChecker
{
    #region Constructor

    public AccountLoginChecker(ILogger logger, IPixaiNavigation pixaiNavigation)
    {
        _pixaiNavigation = pixaiNavigation;
        _logger = logger;
    }

    #endregion


    public bool CheckAccountLogin(UserAccount userAccount,IDriverCreationStrategy driverCreationStrategy)
    {
        using var driver = driverCreationStrategy.CreateDriver();

        _logger.Log("=====Launched Chrome Driver=====", _logger.CreditClaimerLogFilePath);

        _pixaiNavigation.NavigateToUrl(driver, LoginPageUrl);
        _pixaiNavigation.LogIn(driver, userAccount.Email, userAccount.Password);

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(MaxLoginAttemptSeconds));

        _logger.Log($"Logging in to {userAccount.Email}", _logger.CreditClaimerLogFilePath);

        if (wait.Until(drv => drv.Url == MainPageUrl))
        {
            _logger.Log($"The {userAccount.Email} login details are correct\n=====Chrome Drive Closed=====\n",
                _logger.CreditClaimerLogFilePath);
            driver.Quit();
            return true;
        }

        driver.Quit();
        _logger.Log($"The {userAccount.Email} login details are incorrect\n=====Chrome Drive Closed=====\n",
            _logger.CreditClaimerLogFilePath);

        return false;
    }


    public IEnumerable<UserAccount> CheckAllAccountsLogin(IEnumerable<UserAccount> accountsList, IDriverCreationStrategy driverCreationStrategy
        ,CancellationToken token)
    {
        var validAccounts = new List<UserAccount>();

        foreach (var userAccount in accountsList)
        {
            if (token.IsCancellationRequested) return accountsList;

            try
            {
                if (CheckAccountLogin(userAccount,driverCreationStrategy))
                {
                    validAccounts.Add(userAccount);
                    continue;
                }
            }
            catch (Exception)
            {
                continue;
            }

            ValidAccountLogin?.Invoke(this, userAccount);
        }

        return validAccounts;
    }

    private const int MaxLoginAttemptSeconds = 5;

    private const string MainPageUrl = "https://pixai.art/";

    private const string LoginPageUrl = "https://pixai.art/login";

    private readonly IPixaiNavigation _pixaiNavigation;

    private readonly ILogger _logger;

    public event EventHandler<UserAccount> ValidAccountLogin;
}