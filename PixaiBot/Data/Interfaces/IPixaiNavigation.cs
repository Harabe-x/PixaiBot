using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace PixaiBot.Data.Interfaces;

public interface IPixaiNavigation
{
    /// <summary>
    /// Clicks the "Resend Email Verification Link" button on the Pixai website.
    /// </summary>
    /// <param name="searchContext"></param>
    public void ClickResendEmailVerificationLinkButton(ISearchContext searchContext);

    /// <summary>
    /// Navigates the WebDriver instance back to the previous page in the browsing history.
    /// </summary>
    /// <param name="searchContext">The WebDriver instance representing the browser or a frame.</param>
    public void GoBack(IWebDriver searchContext);

    /// <summary>
    ///  Navigates to the Registration form.
    /// </summary>
    /// <param name="searchContext">The WebDriver instance representing the browser or a frame.</param>
    public void NavigateRegistrationPage(ISearchContext searchContext);
   
    /// <summary>
    ///  Goes to the login form page.
    /// </summary>
    /// <param name="searchContext"></param>
    public void GoToLoginPage(ISearchContext searchContext);

    /// <summary>
    ///  Sends the given <paramref name="email"/> and <paramref name="password"/> to the login textboxes on the Pixai website.
    /// </summary>
    /// <param name="searchContext">The WebDriver instance representing the browser or a frame.</param>
    /// <param name="email">Login email</param>
    /// <param name="password">Login Password</param>
    public void SendLoginCredentialsToTextBoxes(ISearchContext searchContext, string email, string password);

    /// <summary>
    ///  Clicks the "Register" button on the Pixai website.
    /// </summary>
    /// <param name="searchContext">The WebDriver instance representing the browser or a frame.</param>
    public void ClickOnRegisterButton(ISearchContext searchContext);
    /// <summary>
    ///  Clicks the "Login" button on the Pixai website.
    /// </summary>
    /// <param name="searchContext">The WebDriver instance representing the browser or a frame.</param>
    public void ClickOnLoginButton(ISearchContext searchContext);

    /// <summary>
    /// Navigates to the "User Profile" page.
    /// </summary>
    /// <param name="searchContext">The WebDriver instance representing the browser or a frame.</param>
    public void NavigateToProfile(ISearchContext searchContext);

    /// <summary>
    /// Navigates to the "Profile Settings" page.
    /// </summary>
    /// <param name="searchContext">The WebDriver instance representing the browser or a frame.</param>
    public void NavigateToProfileSettings(ISearchContext searchContext);


    /// <summary>
    /// Navigates <paramref name="webDriver"/> to the "Credits" tab.
    /// </summary>
    /// <param name="webDriver">The WebDriver instance representing the browser or a frame.</param>
    public void GoToCreditsTab(IWebDriver webDriver);

    /// <summary>
    ///  Clicks the dropdown menu on the Pixai website.
    /// </summary>
    /// <param name="webDriver">The WebDriver instance representing the browser or a frame.</param>
    public void ClickDropdownMenu(IWebDriver webDriver);

    /// <summary>
    ///  Navigates to the "My Work" tab.
    /// </summary>
    /// <param name="webDriver">The WebDriver instance representing the browser or a frame.</param>
    public void NavigateToMyWorkTab(IWebDriver webDriver);

    /// <summary>
    ///  Navigates <paramref name="webDriver"/> to the given <paramref name="url"/>.
    /// </summary>
    /// <param name="webDriver">The WebDriver instance representing the browser or a frame.</param>
    /// <param name="url">Destination url</param>
    public void NavigateToUrl(IWebDriver webDriver, string url);


    /// <summary>
    ///   Clicks the "Claim Credit" button on the Pixai website.
    /// </summary>
    /// <param name="searchContext">The WebDriver instance representing the browser or a frame.</param>
    public void ClickClaimCreditButton(ISearchContext searchContext);
    /// <summary>
    /// Logs in to Pixai using the given credentials.
    /// This method aggregates a pair of different methods contained in this interface.
    /// </summary>
    /// <param name="webDriver">Driver that controls the browser</param>
    /// <param name="email">Login email</param>
    /// <param name="password">Login Password</param>
    public void LogIn(IWebDriver webDriver, string email, string password);
}