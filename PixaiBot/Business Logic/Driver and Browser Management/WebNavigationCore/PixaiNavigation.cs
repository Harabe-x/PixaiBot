using System;
using System.Collections.Generic;
using System.Linq;
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
        ClickElement(searchContext, "*:nth-child(3) *:nth-child(2) > *:nth-child(4)");
    }

    public void GoBack(IWebDriver driver)
    {
        driver.Navigate().Back();
    }

    public void NavigateRegistrationPage(ISearchContext driver)
    {
        _logger.Log("Finding button to navigate to registration form", _logger.CreditClaimerLogFilePath);
        ClickElementWithSpecifiedText(driver, "button", "Register");
    }

    public void GoToLoginPage(ISearchContext driver)
    {
        _logger.Log("Finding button to navigate to registration form", _logger.CreditClaimerLogFilePath);
        ClickElementWithSpecifiedText(driver, "button", "Log in with email");
    }

    public void SendLoginCredentialsToTextBoxes(ISearchContext driver, string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            throw new ArgumentException("Login credentials can't be null or empty");

        _logger.Log("Sending email & password to textboxes ", _logger.CreditClaimerLogFilePath);

        SendKeysToElement(driver, "* > * > *:nth-child(2) > * > *:nth-child(1) > * > *", email);

        SendKeysToElement(driver, "*:nth-child(2) > * > *:nth-child(2) > * > *", password);
    }

    public void ClickOnRegisterButton(ISearchContext driver)
    {
        _logger.Log("Clicking register button", _logger.CreditClaimerLogFilePath);

        ClickElementWithSpecifiedText(driver, "button", "Sign Up");
    }

    public void ClickOnLoginButton(ISearchContext driver)
    {
        _logger.Log("Clicking login button ", _logger.CreditClaimerLogFilePath);

        ClickElementWithSpecifiedText(driver, "button", "Login");
    }

    public void NavigateToProfile(ISearchContext driver)
    {
        _logger.Log("Navigating to profile", _logger.CreditClaimerLogFilePath);

        ClickElement(driver, ".MuiMenuItem-root:nth-child(1)");
    }

    public void NavigateToProfileSettings(ISearchContext driver)
    {
        _logger.Log("Navigating to account model", _logger.CreditClaimerLogFilePath);

        ClickElement(driver, ".MuiMenuItem-root:nth-child(3)");
    }

    public void GoToCreditsTab(IWebDriver driver)
    {
        NavigateToUrl(driver, driver.Url + "/credits");
    }

    public void ClickDropdownMenu(IWebDriver driver)
    {
        ClickElement(driver, ".shrink-0");
    }

    public void NavigateToMyWorkTab(IWebDriver driver)
    {
        NavigateToUrl(driver, driver.Url + "/artwork");
    }

    public void NavigateToUrl(IWebDriver driver, string url)
    {
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

    private static void ClickElement(ISearchContext driver, string cssSelector)
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

    private static void ClickElementWithSpecifiedText(ISearchContext driver, string tagName, string text)
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

    private static void SendKeysToElement(ISearchContext driver, string cssSelector, string keys)
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