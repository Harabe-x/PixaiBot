using PixaiBot.Bussines_Logic.Driver_and_Browser_Management.WebNavigationCore.WebNavigationCoreException;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;
using System.Threading;
using System;

namespace PixaiBot.Bussines_Logic;


public class AccountLoginChecker : IAccountLoginChecker
{
    #region Constructor
    public AccountLoginChecker(ILogger logger, IPixaiNavigation pixaiNavigation, ITcpServerConnector tcpServerConnector)
    {
        _pixaiNavigation = pixaiNavigation;
        _logger = logger;
    }
    #endregion


    /// <summary>
    /// Checks account login credentials 
    /// </summary>
    /// <param name="userAccount"></param>
    /// <returns>True if the account is valid</returns>
    public bool CheckAccountLogin(UserAccount userAccount)
    {
        using var driver = ChromeDriverFactory.CreateDriverForDebug();

        _pixaiNavigation.NavigateToUrl(driver, LoginUrl);
        _pixaiNavigation.LogIn(driver, userAccount.Email, userAccount.Password);

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(MaxLoginAttemptSeconds)); 

        if (wait.Until(drv => drv.Url == MainPageUrl))
        {
            driver.Quit();
            return true;
        }
        driver.Quit();
      
        return false;

    }

    /// <summary>
    /// Checks all accounts login credentials
    /// </summary> 
    /// <param name="accountsList"></param>
    /// <returns>IEnumerable with valid accounts</returns>
    public IEnumerable<UserAccount> CheckAllAccountsLogin(IEnumerable<UserAccount> accountsList, CancellationToken token)
    {
        var validAccounts = new List<UserAccount>();

        foreach (var userAccount in accountsList)
        {
            if (token.IsCancellationRequested) return accountsList;

            try
            {
                if (CheckAccountLogin(userAccount))
                {
                    validAccounts.Add(userAccount);
                    continue;
                }
            }
            catch (ChromeDriverException)
            {
                continue;
            }

            ValidAccountLogin?.Invoke(this, userAccount);
        }
        return validAccounts;
    }
    private const int MaxLoginAttemptSeconds = 5;

    private const string MainPageUrl = "https://pixai.art/";

    private const string LoginUrl = "https://pixai.art/login";

    private readonly IPixaiNavigation _pixaiNavigation;

    private readonly ILogger _logger; // TODO: Add logging to this class

    public event EventHandler<UserAccount> ValidAccountLogin;
}