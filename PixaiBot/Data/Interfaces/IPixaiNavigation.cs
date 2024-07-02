using OpenQA.Selenium;

namespace PixaiBot.Data.Interfaces;

public interface IPixaiNavigation
{
    /// <summary>
    ///     Clicks the "Resend Email Verification Link" button on the Pixai website.
    /// </summary>
    /// <param name="searchContext"></param>
    public void ClickResendEmailVerificationLinkButton(ISearchContext searchContext);

    /// <summary>
    ///     Navigates the WebDriver instance back to the previous page in the browsing history.
    /// </summary>
    /// <param name="searchContext">The WebDriver instance representing the browser or a frame.</param>
    public void GoBack(IWebDriver searchContext);

    /// <summary>
    ///     Navigates to the Registration form.
    /// </summary>
    /// <param name="searchContext">The WebDriver instance representing the browser or a frame.</param>
    public void NavigateToRegistrationPage(ISearchContext searchContext);

    /// <summary>
    ///     Goes to the login form page.
    /// </summary>
    /// <param name="searchContext"></param>
    public void GoToLoginPage(ISearchContext searchContext);

    /// <summary>
    ///     Sends the given <paramref name="email" /> and <paramref name="password" /> to the login textboxes on the Pixai
    ///     website.
    /// </summary>
    /// <param name="searchContext">The WebDriver instance representing the browser or a frame.</param>
    /// <param name="email">Login email</param>
    /// <param name="password">Login Password</param>
    public void SendLoginCredentialsToTextBoxes(ISearchContext searchContext, string email, string password);

    /// <summary>
    ///     Clicks the "Register" button on the Pixai website.
    /// </summary>
    /// <param name="searchContext">The WebDriver instance representing the browser or a frame.</param>
    public void ClickOnRegisterButton(ISearchContext searchContext);

    /// <summary>
    ///     Clicks the "Login" button on the Pixai website.
    /// </summary>
    /// <param name="searchContext">The WebDriver instance representing the browser or a frame.</param>
    public void ClickOnLoginButton(ISearchContext searchContext);

    /// <summary>
    ///     Navigates to the "User Profile" page.
    /// </summary>
    /// <param name="searchContext">The WebDriver instance representing the browser or a frame.</param>
    public void NavigateToProfile(ISearchContext searchContext);

    /// <summary>
    ///     Navigates to the "Profile Settings" page.
    /// </summary>
    /// <param name="searchContext">The WebDriver instance representing the browser or a frame.</param>
    public void NavigateToProfileSettings(IWebDriver searchContext);


    /// <summary>
    ///     Navigates <paramref name="webDriver" /> to the "Credits" tab.
    /// </summary>
    /// <param name="webDriver">The WebDriver instance representing the browser or a frame.</param>
    public void NavigateToCreditsTab(IWebDriver webDriver);

    /// <summary>
    ///     Clicks the dropdown menu on the Pixai website.
    /// </summary>
    /// <param name="webDriver">The WebDriver instance representing the browser or a frame.</param>
    public void ClickDropdownMenu(IWebDriver webDriver);

    /// <summary>
    ///     Navigates to the "My Work" tab.
    /// </summary>
    /// <param name="webDriver">The WebDriver instance representing the browser or a frame.</param>
    public void NavigateToMyWorkTab(IWebDriver webDriver);

    /// <summary>
    ///     Navigates <paramref name="webDriver" /> to the given <paramref name="url" />.
    /// </summary>
    /// <param name="webDriver">The WebDriver instance representing the browser or a frame.</param>
    /// <param name="url">Destination url</param>
    public void NavigateToUrl(IWebDriver webDriver, string url);


    /// <summary>
    ///     Clicks the "Claim Credit" button on the Pixai website.
    /// </summary>
    /// <param name="searchContext">The WebDriver instance representing the browser or a frame.</param>
    public void ClickClaimCreditButton(ISearchContext searchContext);

    /// <summary>
    ///     Logs in to Pixai using the given credentials.
    ///     This method aggregates a pair of different methods contained in this interface.
    /// </summary>
    /// <param name="webDriver">Driver that controls the browser</param>
    /// <param name="email">Login email</param>
    /// <param name="password">Login Password</param>
    public void LogIn(IWebDriver webDriver, string email, string password);

    /// <summary>
    ///     searches for and clicks on the element with the selected <paramref name="cssSelector" />
    /// </summary>
    /// <param name="searchContext">The WebDriver instance representing the browser or a frame.</param>
    /// <param name="cssSelector">  The CSS selector used to locate the desired element within the specified search context.</param>
    public void ClickElement(ISearchContext searchContext, string cssSelector);

    /// <summary>
    /// </summary>
    /// <param name="searchContext">The WebDriver instance representing the browser or a frame.</param>
    /// <param name="cssSelector"> The CSS selector used to locate the desired element within the specified search context.</param>
    /// <param name="keys">The text to be sent to the element</param>
    public void SendKeysToElement(ISearchContext searchContext, string cssSelector, string keys);


    /// <summary>
    ///     Closes the pop-up window that appears after login.
    ///     This method will be useless after Pixai removes the  popup after login
    /// </summary>
    /// <param name="searchContext">>The WebDriver instance representing the browser or a frame.</param>
    public void ClosePopup(ISearchContext searchContext);

    /// <summary>
    ///     Clicks the element with the selected <paramref name="tagName" /> and <paramref name="text" />
    /// </summary>
    /// <param name="searchContext">The WebDriver instance representing the browser or a frame.</param>
    /// <param name="tagName">The HTML tag name of the element to be located and clicked.</param>
    /// <param name="text"> The text content that the located element should contain.</param>
    public void ClickElement(ISearchContext searchContext, string tagName, string text);

    /// <summary>
    ///     Retrieves and returns the HTML element located by the specified <paramref name="cssSelector" /> within the provided
    ///     search context.
    /// </summary>
    /// <param name="searchContext">
    ///     The WebDriver instance representing the browser or a frame in which the element is to be
    ///     searched.
    /// </param>
    /// <param name="cssSelector">The CSS selector used to locate the desired HTML element within the specified search context.</param>
    /// <returns>The <see cref="IWebElement" /> representing the located HTML element.</returns>
    public IWebElement GetElementByCssSelector(ISearchContext searchContext, string cssSelector);

    /// <summary>
    ///     Retrieves and returns the HTML element with the specified <paramref name="tagName" /> and containing the specified
    ///     <paramref name="text" />
    ///     within the provided search context.
    /// </summary>
    /// <param name="searchContext">
    ///     The WebDriver instance representing the browser or a frame in which the element is to be
    ///     searched.
    /// </param>
    /// <param name="tagName">The HTML tag name of the element to be located.</param>
    /// <param name="text">The text content that the located element should contain.</param>
    /// <returns>The <see cref="IWebElement" /> representing the located HTML element.</returns>
    public IWebElement GetElementByText(ISearchContext searchContext, string tagName, string text);


    /// <summary>
    ///     Navigates to Account tab in Edit Profile Page
    /// </summary>
    /// <param name="searchContext"></param>
    public void NavigateToAccountTabInEditProfilePage(ISearchContext searchContext);
}