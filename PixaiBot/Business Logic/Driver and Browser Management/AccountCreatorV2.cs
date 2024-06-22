using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PixaiBot.Business_Logic.Driver_and_Browser_Management.Driver_Creation_Strategy;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Models;

namespace PixaiBot.Business_Logic.Driver_and_Browser_Management;

internal class AccountCreatorV2 : IAccountCreator
{
    #region Constructor

    public AccountCreatorV2(IPixaiNavigation pixaiNavigation, ITempMailApiManager tempMailApiManager,
        ILoginCredentialsMaker loginCredentialsMaker, ILogger logger, IProxyManager proxyManager)
    {
        _proxyManager = proxyManager;
        _tempMailApiManager = tempMailApiManager;
        _logger = logger;
        _loginCredentialsMaker = loginCredentialsMaker;
        _pixaiNavigation = pixaiNavigation;
    }

    #endregion

    #region Methods

    public void CreateAccounts(int amount, string tempMailApiKey, bool shouldVerifyEmail,
        IDriverCreationStrategy driverCreationStrategy, TimeSpan interval, CancellationToken token)
    {
        _logger.Log("The account creation process has started", _logger.ApplicationLogFilePath);


        if (tempMailApiKey == null) tempMailApiKey = string.Empty;
        
        
        for (var i = 0; i < amount; i++)
        {
            if (token.IsCancellationRequested) return;

            var driver = driverCreationStrategy.CreateDriver();

            _logger.Log("=====Launched Chrome Driver=====", _logger.CreditClaimerLogFilePath);

            try
            {
                CreateAccount(driver, shouldVerifyEmail, tempMailApiKey);
            }
            catch (Exception e)
            {
                _logger.Log("Chrome drive threw exception\n" + e.Message, _logger.CreditClaimerLogFilePath);
                ErrorOccurred?.Invoke(this, $"{e.InnerException.GetType()}");
                continue;
            }
            finally
            {
                driver.Quit();
            }

            if (amount < 2) return;

            Thread.Sleep(interval);
        }

        _logger.Log("The account creation process has ended", _logger.CreditClaimerLogFilePath);
    }

    private void CreateAccount(IWebDriver driver, bool shouldVerifyEmail, string tempMailApiKey)
    {
        _logger.Log("Creating account login details", _logger.CreditClaimerLogFilePath);


        var email = shouldVerifyEmail
            ? _loginCredentialsMaker.GenerateEmail(tempMailApiKey)
            : _loginCredentialsMaker.GenerateEmail();

        var password = _loginCredentialsMaker.GeneratePassword();

        _logger.Log("Creating account login details", _logger.CreditClaimerLogFilePath);

        _pixaiNavigation.NavigateToUrl(driver, RegistrationPageUrl);
        _pixaiNavigation.NavigateToRegistrationPage(driver);
        _pixaiNavigation.SendLoginCredentialsToTextBoxes(driver, email, password);
        _pixaiNavigation.ClickOnRegisterButton(driver);
        _logger.Log("Registering an account", _logger.CreditClaimerLogFilePath);

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(MaxRegisterAttemptSeconds));

        try
        {
            wait.Until(drv => drv.Url != RegistrationPageUrl);
        }
        catch (WebDriverTimeoutException)
        {
            ErrorOccurred?.Invoke(this, "An error occurred while creating your account");
            _logger.Log("Account registration failed\n=====Chrome Drive Closed=====\n",
                _logger.CreditClaimerLogFilePath);
            driver.Quit();
            return;
        }


        var userAccount = new UserAccount { Email = email, Password = password };

        AccountCreated?.Invoke(this, userAccount);

        if (!shouldVerifyEmail) return;

        _pixaiNavigation.ClosePopup(driver);

        _logger.Log("Trying to confirm email", _logger.CreditClaimerLogFilePath);

        _pixaiNavigation.NavigateToProfileSettings(driver);
        _pixaiNavigation.ClickResendEmailVerificationLinkButton(driver);

        VerifyEmail(userAccount, driver, tempMailApiKey);
    }

    private void VerifyEmail(UserAccount userAccount, IWebDriver driver, string tempMailApiKey)
    {
        var verificationLink = string.Empty;
        const int maxAttempts = 10;
        var attemptCount = 0;

        while (string.IsNullOrEmpty(verificationLink) && attemptCount < maxAttempts)
        {
            verificationLink = _tempMailApiManager.GetVerificationLink(userAccount.Email, tempMailApiKey);
            if (!string.IsNullOrEmpty(verificationLink)) continue;
            Thread.Sleep(TimeSpan.FromSeconds(EmailVerificationLinkWaitTime));
            attemptCount++;
        }

        if (string.IsNullOrEmpty(verificationLink))
        {
            _logger.Log("Email verification link not found, invalid Api Key\n=====Chrome Drive Closed=====\n",
                _logger.CreditClaimerLogFilePath);
            ErrorOccurred?.Invoke(this, "Invalid Api Key");
            return;
        }

        driver.Navigate().GoToUrl(verificationLink);

        Thread.Sleep(TimeSpan.FromSeconds(2.5));

        _logger.Log("Email verified\n=====Chrome Drive Closed=====\n", _logger.CreditClaimerLogFilePath);
    }

    #endregion

    #region Fields

    public event EventHandler<UserAccount>? AccountCreated;

    private readonly IPixaiNavigation _pixaiNavigation;

    public event EventHandler<string>? ErrorOccurred;

    private const string RegistrationPageUrl = "https://pixai.art/sign-up";

    private const int MaxRegisterAttemptSeconds = 5;

    private const int EmailVerificationLinkWaitTime = 5;

    private readonly IProxyManager _proxyManager;

    private readonly ITempMailApiManager _tempMailApiManager;

    private readonly ILoginCredentialsMaker _loginCredentialsMaker;

    private readonly ILogger _logger;

    #endregion
}