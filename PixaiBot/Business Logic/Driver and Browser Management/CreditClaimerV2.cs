using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PixaiBot.Business_Logic.Driver_and_Browser_Management.Driver_Creation_Strategy;
using PixaiBot.Business_Logic.Driver_and_Browser_Management.WebNavigationCore.WebNavigationCoreException;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Models;

namespace PixaiBot.Business_Logic.Driver_and_Browser_Management;

internal class CreditClaimerV2 : ICreditClaimer
{
    #region Constructor

    public CreditClaimerV2(ILogger logger, IPixaiNavigation pixaiNavigation)
    {
        _logger = logger;
        _pixaiNavigation = pixaiNavigation;
    }

    #endregion

    #region Methods

    public void ClaimCredits(UserAccount userAccount, IDriverCreationStrategy driverCreationStrategy)
    {
        using var driver = driverCreationStrategy.CreateDriver();

        _logger.Log("=====Launched Chrome Driver=====", _logger.CreditClaimerLogFilePath);

        _pixaiNavigation.NavigateToUrl(driver, LoginUrl);
        _pixaiNavigation.LogIn(driver, userAccount.Email, userAccount.Password);


        _logger.Log($"Logging in to {userAccount.Email}", _logger.CreditClaimerLogFilePath);
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(MaxLoginAttemptSeconds));

        try
        {
            wait.Until(drv => drv.Url != LoginUrl);
        }
        catch (WebDriverTimeoutException)
        {
            ErrorOccurred?.Invoke(this, "Invalid Login Credentials");
            _logger.Log("=====Chrome Drive Closed=====\n", _logger.CreditClaimerLogFilePath);
            driver.Quit();
            return;
        } 
        
        // Code below will change with popup removal 
        try
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(2500));
            _pixaiNavigation.ClaimCreditsUsingPopup(driver);
            Thread.Sleep(TimeSpan.FromMilliseconds(1000));
        }
        catch (StaleElementReferenceException)
        {
            _logger.Log("Credits was already claimed on this account", _logger.CreditClaimerLogFilePath);
            _logger.Log("=====Chrome Drive Closed=====\n", _logger.CreditClaimerLogFilePath);   
            CreditsAlreadyClaimed?.Invoke(this, userAccount);
            driver.Quit();
            return;
        }
        
        
        
        driver.Quit();
        _logger.Log($"Credits claimed for {userAccount.Email}", _logger.CreditClaimerLogFilePath);
        CreditsClaimed?.Invoke(this, userAccount);
        _logger.Log("=====Chrome Drive Closed=====\n", _logger.CreditClaimerLogFilePath);

        }


    public void ClaimCreditsForAllAccounts(IEnumerable<UserAccount> accounts,
        IDriverCreationStrategy driverCreationStrategy, CancellationToken cancellationToken)
    {
        foreach (var account in accounts)
        {
            if (cancellationToken.IsCancellationRequested) return;

            ProcessStartedForAccount?.Invoke(this, account);

            try
            {
                ClaimCredits(account, driverCreationStrategy);
            }
            catch (ElementClickInterceptedException e)
            {
                // This exception may occur in headless mode when the claim button is clicked many times.
                // It seems to be related to the headless environment, possibly due to rapid claim button clicks.
                // I think it can be safely ignored because the credits are successfully claimed regardless.
                _logger.Log(e.Message, _logger.CreditClaimerLogFilePath);
            }
            catch (InvalidPageContentException e)
            {
                _logger.Log(e.Message, _logger.CreditClaimerLogFilePath);
                ErrorOccurred?.Invoke(this, e.InnerException?.GetType().ToString() ?? "Error occurred");
            }
            catch (Exception e)
            {
                _logger.Log(e.Message, _logger.CreditClaimerLogFilePath);
                ErrorOccurred?.Invoke(this, e.InnerException?.GetType().ToString() ?? "Error occurred");
            }
        }
    }

    #endregion

    #region Fields

    private readonly ILogger _logger;

    private readonly IPixaiNavigation _pixaiNavigation;

    private const string LoginUrl = "https://pixai.art/login";

    private const int MaxTries = 5;

    private const int MaxLoginAttemptSeconds = 5;

    public event EventHandler<string> ErrorOccurred;

    public event EventHandler<UserAccount>? CreditsClaimed;

    public event EventHandler<UserAccount>? CreditsAlreadyClaimed;

    public event EventHandler<UserAccount>? ProcessStartedForAccount;

    #endregion
}