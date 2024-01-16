using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using PixaiBot.Business_Logic.Driver_and_Browser_Management.Driver_Creation_Strategy;
using PixaiBot.Business_Logic.Extension;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Models;

namespace PixaiBot.Business_Logic.Driver_and_Browser_Management;

internal class AccountInfoLogger : IAccountInfoLogger
{
    #region Constructor

    public AccountInfoLogger(IPixaiNavigation pixaiNavigation, IPixaiDataReader pixaiDataReader, ILogger logger)
    {
        _pixaiNavigation = pixaiNavigation;
        _pixaiDataReader = pixaiDataReader;
        _logger = logger;

        _stringBuilder = new StringBuilder();
    }

    #endregion

    #region Methods

    public void ClearStringBuilderContent()
    {
        _stringBuilder.Clear();
    }

   

    public string StartLoggingAccountsInfo(IEnumerable<UserAccount> userAccountsList,
        IDriverCreationStrategy driverCreationStrategy, IAccountInfoLoggerSettings settings,
        CancellationToken cancellationToken)
    {
        _logger.Log($"Logging information about {userAccountsList.Count()} accounts", _logger.CreditClaimerLogFilePath);
        foreach (var account in userAccountsList)
        {
            if (cancellationToken.IsCancellationRequested) return _stringBuilder.ToString();
            try
            {
                LogAccountInfo(account,settings,driverCreationStrategy);
            }
            catch (Exception e)
            {
                _logger.Log($"Error occurred, Error message : {e.Message}", _logger.CreditClaimerLogFilePath);
                continue;
            }
        }

        return _stringBuilder.ToString();
    }

    private void LogAccountInfo(UserAccount account, IAccountInfoLoggerSettings settings, IDriverCreationStrategy driverCreationStrategy)
    {
        _logger.Log("=====Launched Chrome Driver=====", _logger.CreditClaimerLogFilePath);

        using var driver = driverCreationStrategy.CreateDriver();
        var internalStringBuilder = new StringBuilder();
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(MaxLoginAttemptSeconds));

        _logger.Log($"Logging in to {account.Email}", _logger.CreditClaimerLogFilePath);

        _pixaiNavigation.NavigateToUrl(driver, LoginPageUrl);
        _pixaiNavigation.LogIn(driver, account.Email, account.Password);
        internalStringBuilder.AppendLine(
            $"======Account Info======\nEmail : {account.Email}\nPassword : {account.Password}");

        if (!wait.Until(drv => drv.Url == MainPageUrl))
        {
            _logger.Log("Login Failed\n=====Chrome Driver Closed=====\n", _logger.CreditClaimerLogFilePath);
            internalStringBuilder.AppendLine($"Login Operation Status : Failed\n==============================");
            return;
        }

        _logger.Log($"Reading account data", _logger.CreditClaimerLogFilePath);

        while (!driver.Url.Contains('@'))
        {
            _pixaiNavigation.ClickDropdownMenu(driver);
            _pixaiNavigation.NavigateToProfile(driver);
        }

        Thread.Sleep(TimeSpan.FromSeconds(DynamicDataLoadDelay));

        internalStringBuilder.AppendLineIf(settings.ShouldLogAccountUsername,
            $"Username : {_pixaiDataReader.GetUsername(driver)}");
        internalStringBuilder.AppendLineIf(settings.ShouldLogAccountCredits,
            $"Credits : {_pixaiDataReader.GetCreditsCount(driver)}");
        internalStringBuilder.AppendLineIf(settings.ShouldLogFollowersCount,
            $"Followers Count : {_pixaiDataReader.GetFollowersCount(driver)}");
        internalStringBuilder.AppendLineIf(settings.ShouldLogFollowingCount,
            $"Following Count : {_pixaiDataReader.GetFollowingCount(driver)}");

        _pixaiNavigation.NavigateToUrl(driver, UserProfileUrl);

        Thread.Sleep(TimeSpan.FromSeconds(DynamicDataLoadDelay));

        internalStringBuilder.AppendLineIf(settings.ShouldLogEmailVerificationStatus,
            $"Email Verification Status : {_pixaiDataReader.GetEmailVerificationStatus(driver)}");
        internalStringBuilder.AppendLineIf(settings.ShouldLogAccountId,
            $"Account Id : {_pixaiDataReader.GetAccountId(driver)}");

        internalStringBuilder.AppendLine("===============");
        _stringBuilder.AppendLine(internalStringBuilder.ToString());
        driver.Quit();
        _logger.Log("=====Chrome Driver Closed=====\n", _logger.CreditClaimerLogFilePath);
    }

    #endregion

    #region Fields

    private readonly IPixaiNavigation _pixaiNavigation;

    private readonly IPixaiDataReader _pixaiDataReader;

    private readonly ILogger _logger;

    private readonly StringBuilder _stringBuilder;

    private const string LoginPageUrl = "https://pixai.art/login";

    private const string MainPageUrl = "https://pixai.art/";

    private const string UserProfileUrl = "https://pixai.art/profile/edit";

    private const int DynamicDataLoadDelay = 1;

    private const int MaxLoginAttemptSeconds = 5;

    #endregion
}