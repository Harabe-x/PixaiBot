using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using PixaiBot.Business_Logic.Extension;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Models;

namespace PixaiBot.Business_Logic.Driver_and_Browser_Management;

internal class AccountsInfoLogger : IAccountsInfoLogger
{
    #region Constructor

    public AccountsInfoLogger(IPixaiNavigation pixaiNavigation, IPixaiDataReader pixaiDataReader, ILogger logger)
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
        IAccountInfoLoggerSettings settings,
        CancellationToken cancellationToken)
    {
        foreach (var account in userAccountsList)
        {
            if (cancellationToken.IsCancellationRequested) return _stringBuilder.ToString();


            try
            {
                LogAccountInfo(account, settings);
            }
            catch (Exception e)
            {
                _logger.Log($"Error occurred, Error message : {e.Message}", _logger.ApplicationLogFilePath);
                continue;
            }
        }

        return _stringBuilder.ToString();
    }

    private void LogAccountInfo(UserAccount account, IAccountInfoLoggerSettings settings)
    {
        using var driver = ChromeDriverFactory.CreateDriver();
        var internalStringBuilder = new StringBuilder();
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(MaxLoginAttemptSeconds));

        _pixaiNavigation.NavigateToUrl(driver, LoginPageUrl);
        _pixaiNavigation.LogIn(driver, account.Email, account.Password);
        internalStringBuilder.AppendLine(
            $"======Account Info======\nEmail : {account.Email}\nPassword : {account.Password}");

        if (!wait.Until(drv => drv.Url == MainPageUrl))
        {
            internalStringBuilder.AppendLine($"Login OperationStatus : Failed\n==============================");
            return;
        }

        while (!driver.Url.Contains('@'))
        {
            _pixaiNavigation.ClickDropdownMenu(driver);
            _pixaiNavigation.NavigateToProfile(driver);
        }

        Thread.Sleep(TimeSpan.FromSeconds(DynamicDataLoadDelay));

        internalStringBuilder.AppendLineIf(settings.ShouldLogAccountUsername, $"Username : {_pixaiDataReader.GetUsername(driver)}");
        internalStringBuilder.AppendLineIf(settings.ShouldLogAccountCredits, $"Credits : {_pixaiDataReader.GetCreditsCount(driver)}");
        internalStringBuilder.AppendLineIf(settings.ShouldLogFollowersCount, $"Followers Count : {_pixaiDataReader.GetFollowersCount(driver)}");
        internalStringBuilder.AppendLineIf(settings.ShouldLogFollowingCount, $"Following Count : {_pixaiDataReader.GetFollowingCount(driver)}");

        _pixaiNavigation.NavigateToUrl(driver, UserProfileUrl);

        Thread.Sleep(TimeSpan.FromSeconds(DynamicDataLoadDelay));

        internalStringBuilder.AppendLineIf(settings.ShouldLogEmailVerificationStatus, $"Email Verification OperationStatus : {_pixaiDataReader.GetEmailVerificationStatus(driver)}");
        internalStringBuilder.AppendLineIf(settings.ShouldLogAccountId, $"Account Id : {_pixaiDataReader.GetAccountId(driver)}");

        internalStringBuilder.AppendLine("===============");
        _stringBuilder.AppendLine(internalStringBuilder.ToString());
        driver.Quit();
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