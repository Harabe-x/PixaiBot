﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenQA.Selenium;
using PixaiBot.Business_Logic.Driver_and_Browser_Management.WebNavigationCore.WebNavigationCoreException;
using PixaiBot.Data.Interfaces;

namespace PixaiBot.Business_Logic.Driver_and_Browser_Management.WebNavigationCore;

internal class PixaiNavigation : IPixaiNavigation
{
    #region Constructor

    public PixaiNavigation(ILogger logger)
    {
        _logger = logger;
    }

    #endregion


    #region Methods

    #region IPixaiNavigation

    public void ClickResendEmailVerificationLinkButton(ISearchContext searchContext)
    {
        _logger.Log("Clicking resend email verification link button", _logger.CreditClaimerLogFilePath);
        ClickElement(searchContext, "*:nth-child(3) *:nth-child(2) > *:nth-child(4)");
    }

    public void GoBack(IWebDriver driver)
    {
        _logger.Log("Going back", _logger.CreditClaimerLogFilePath);
        driver.Navigate().Back();
    }

    public void NavigateToRegistrationPage(ISearchContext searchContext)
    {
        _logger.Log("Finding button to navigate to registration form", _logger.CreditClaimerLogFilePath);
        ClickElement(searchContext, ".MuiButton-text");
    }

    public void GoToLoginPage(ISearchContext searchContext)
    {
        _logger.Log("Finding button to navigate to registration form", _logger.CreditClaimerLogFilePath);
        ClickElement(searchContext, "button", "Log in with email");
    }

    public void SendLoginCredentialsToTextBoxes(ISearchContext searchContext, string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            throw new ArgumentException("Login credentials can't be null or empty");

        _logger.Log("Sending email & password to textboxes ", _logger.CreditClaimerLogFilePath);

        SendKeysToElement(searchContext, "* > * > *:nth-child(2) > * > *:nth-child(1) > * > *", email);

        SendKeysToElement(searchContext, "*:nth-child(2) > * > *:nth-child(2) > * > *", password);
    }

    public void ClickOnRegisterButton(ISearchContext searchContext)
    {
        _logger.Log("Clicking register button", _logger.CreditClaimerLogFilePath);

        ClickElement(searchContext, "#\\:r2\\:");
    }

    public void ClickOnLoginButton(ISearchContext searchContext)
    {
        _logger.Log("Clicking login button ", _logger.CreditClaimerLogFilePath);

        ClickElement(searchContext, "button", "Login");
    }

    public void NavigateToProfile(ISearchContext searchContext)
    {
        _logger.Log("Navigating to profile", _logger.CreditClaimerLogFilePath);

        ClickElement(searchContext, ".MuiMenuItem-root:nth-child(1)");
    }

    public void NavigateToProfileSettings(IWebDriver driver)
    {
        _logger.Log("Navigating to account model", _logger.CreditClaimerLogFilePath);
        NavigateToUrl(driver, "https://pixai.art/profile/edit");
    }

    public void NavigateToCreditsTab(IWebDriver driver)
    {
        _logger.Log("Clicking dropdown menu", _logger.CreditClaimerLogFilePath);

        NavigateToUrl(driver, driver.Url + "/credits");
    }

    public void ClickDropdownMenu(IWebDriver driver)
    {
        _logger.Log("Clicking dropdown menu", _logger.CreditClaimerLogFilePath);
        ClickElement(driver, ".shrink-0");
    }

    public void NavigateToMyWorkTab(IWebDriver driver)
    {
        NavigateToUrl(driver, driver.Url + "/artwork");
    }

    public void NavigateToUrl(IWebDriver driver, string url)
    {
        _logger.Log($"Navigating to {url}", _logger.CreditClaimerLogFilePath);
        driver.Navigate().GoToUrl(url);
    }

    public void ClickClaimCreditButton(ISearchContext searchContext)
    {
        try
        {
            ClickElement(searchContext, ".py-3:nth-child(2) .relative");
        }
        catch (ElementClickInterceptedException)
        {
            ClickClaimCreditButton(searchContext);
        }
    }

    public void LogIn(IWebDriver driver, string email, string password)
    {
        GoToLoginPage(driver);
        SendLoginCredentialsToTextBoxes(driver, email, password);
        ClickOnLoginButton(driver);
    }

    #endregion

    public void ClickElement(ISearchContext driver, string cssSelector)
    {
        try
        {
            driver.FindElement(By.CssSelector(cssSelector)).Click();
        }
        catch (Exception e)
        {
            throw new ChromeDriverException("ChromeDriver exception occurred", e);
        }
    }

    public void ClickElement(ISearchContext driver, string tagName, string text)
    {
        try
        {
            IReadOnlyCollection<IWebElement> buttons = driver.FindElements(By.TagName(tagName));
            buttons.FirstOrDefault(x => x.Text == text)?.Click();
        }
        catch (Exception e)
        {
            throw new ChromeDriverException("ChromeDriver exception occurred", e);
        }
    }

    public IWebElement GetElement(ISearchContext searchContext, string cssSelector)
    {
        try
        {
            return searchContext.FindElement(By.CssSelector(cssSelector));
        }
        catch (Exception e)
        {
            throw new ChromeDriverException("ChromeDriver exception occurred", e);
        }
    }

    public IWebElement GetElement(ISearchContext searchContext, string tagName, string text)
    {
        try
        {
            return searchContext.FindElements(By.TagName(tagName)).FirstOrDefault(x => x.Text == text) ??
                   throw new InvalidOperationException();
        }
        catch (Exception e)
        {
            throw new ChromeDriverException("ChromeDriver exception occurred", e);
        }
    }

    public void SendKeysToElement(ISearchContext driver, string cssSelector, string keys)
    {
        try
        {
            var element = driver.FindElement(By.CssSelector(cssSelector));
            element.Click();
            element.SendKeys(keys);
        }
        catch (Exception e)
        {
            throw new ChromeDriverException("ChromeDriver exception occurred", e);
        }
    }

    #endregion


    #region Fields

    private readonly ILogger _logger;

    #endregion
}