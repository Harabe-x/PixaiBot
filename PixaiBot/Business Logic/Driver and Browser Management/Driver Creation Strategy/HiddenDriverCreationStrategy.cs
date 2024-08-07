﻿using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace PixaiBot.Business_Logic.Driver_and_Browser_Management.Driver_Creation_Strategy;

internal class HiddenDriverCreationStrategy : IDriverCreationStrategy
{
    public IWebDriver CreateDriver()
    {
        var options = new ChromeOptions();

        options.AddArguments("--window-position=-32000,-32000", "--disable-crash-reporter", "--disable-gpu",
            "--disable-crash-reporter", "--disable-search-engine-choice-screen");
        options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);

        var service = ChromeDriverService.CreateDefaultService();

        service.HideCommandPromptWindow = true;

        var driver = new ChromeDriver(service, options);

        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

        return driver;
    }
}