﻿using System;
using OpenQA.Selenium;
using PixaiBot.Business_Logic.Driver_and_Browser_Management.WebNavigationCore.WebNavigationCoreException;
using PixaiBot.Data.Interfaces;

namespace PixaiBot.Business_Logic.Driver_and_Browser_Management.WebNavigationCore;

internal class PixaiDataReader : IPixaiDataReader
{
    #region Fields

    private readonly ILogger _logger;

    #endregion

    #region Constructor

    public PixaiDataReader(ILogger logger)
    {
        _logger = logger;
    }

    #endregion

    #region Methods

    public string GetAccountId(ISearchContext searchContext)
    {
        _logger.Log("Reading Account Id", _logger.CreditClaimerLogFilePath);
        return GetWebElementText(searchContext,
            ".MuiFormHelperText-filled .font-bold");
    }

    public string GetUsername(ISearchContext searchContext)
    {
        _logger.Log("Reading Username", _logger.CreditClaimerLogFilePath);
        return GetWebElementText(searchContext, ".font-semibold.text-xl");
    }

    public string GetCreditsCount(ISearchContext searchContext)
    {
        _logger.Log("Reading Credits count", _logger.CreditClaimerLogFilePath);
        return GetWebElementText(searchContext, ".items-center .flex.text-theme-primary > span");
    }

    public string GetEmailVerificationStatus(ISearchContext searchContext)
    {
        _logger.Log("Reading Email Verification OperationStatus", _logger.CreditClaimerLogFilePath);
        return GetWebElementText(searchContext, "div > span:nth-child(2)");
    }

    public string GetFollowersCount(ISearchContext searchContext)
    {
        _logger.Log("Reading Followers Count", _logger.CreditClaimerLogFilePath);
        return GetWebElementText(searchContext, ".flex.gap-6.items-center > a:nth-child(2) > span");
    }

    public string GetFollowingCount(ISearchContext searchContext)
    {
        _logger.Log("Reading Following Count", _logger.CreditClaimerLogFilePath);
        return GetWebElementText(searchContext, ".flex.gap-6.items-center > a:nth-child(1) > span");
    }

    private static string GetWebElementText(ISearchContext searchContext, string cssSelector)
    {
        IWebElement element;
        try
        {
            element = searchContext.FindElement(By.CssSelector(cssSelector));
        }
        catch (Exception e)
        {
            throw new ChromeDriverException("SearchContext error occurred while trying to get element text",
                e);
        }

        if (string.IsNullOrEmpty(element.Text)) throw new EmptyTextException("There is no text in this element");

        return element.Text;
    }

    private static string GetWebElementAttribute(ISearchContext searchContext, string cssSelector,
        string attributeName)
    {
        IWebElement element;
        try
        {
            element = searchContext.FindElement(By.CssSelector(cssSelector));
        }
        catch (Exception e)
        {
            throw new ChromeDriverException("SearchContext error occurred while trying to get element text",
                e);
        }

        if (string.IsNullOrEmpty(element.GetAttribute(attributeName)))
            throw new EmptyTextException("There is no text in this element");

        return element.Text;
    }

    #endregion
}