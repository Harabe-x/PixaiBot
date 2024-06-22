using OpenQA.Selenium;

namespace PixaiBot.Data.Interfaces;

internal interface IPixaiDataReader
{
    /// <summary>
    ///     Read the account id from the search context.
    /// </summary>
    /// <param name="searchContext">The WebDriver instance representing the browser or a frame.</param>
    /// <returns>Account id</returns>
    public string GetAccountId(ISearchContext searchContext);

    /// <summary>
    ///     Read the username from the search context.
    /// </summary>
    /// <param name="searchContext">The WebDriver instance representing the browser or a frame.</param>
    /// <returns>Username</returns>
    public string GetUsername(ISearchContext searchContext);

    /// <summary>
    ///     Read the credits count from the search context.
    /// </summary>
    /// <param name="searchContext">The WebDriver instance representing the browser or a frame.</param>
    /// <returns>Credits count</returns>
    public string GetCreditsCount(ISearchContext searchContext);

    /// <summary>
    ///     Read the email verification status from the search context.
    /// </summary>
    /// <param name="searchContext">The WebDriver instance representing the browser or a frame.</param>
    /// <returns>Email verification status</returns>
    public string GetEmailVerificationStatus(ISearchContext searchContext);

    /// <summary>
    ///     Read the followers count from the search context.
    /// </summary>
    /// <param name="searchContext">The WebDriver instance representing the browser or a frame.</param>
    /// <returns>Followers Count</returns>
    public string GetFollowersCount(ISearchContext searchContext);

    /// <summary>
    ///     Read the following count from the search context.
    /// </summary>
    /// <param name="searchContext">The WebDriver instance representing the browser or a frame.</param>
    /// <returns>Following count</returns>
    public string GetFollowingCount(ISearchContext searchContext);
}