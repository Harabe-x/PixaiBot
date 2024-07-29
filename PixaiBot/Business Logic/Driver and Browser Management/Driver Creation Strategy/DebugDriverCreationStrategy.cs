using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace PixaiBot.Business_Logic.Driver_and_Browser_Management.Driver_Creation_Strategy;

internal class DebugDriverCreationStrategy : IDriverCreationStrategy
{
    public IWebDriver CreateDriver()
    {
        var service = ChromeDriverService.CreateDefaultService();

        var options = new ChromeOptions();

        options.AddArguments("--disable-crash-reporter", "--disable-gpu",
            "--disable-crash-reporter", "--disable-search-engine-choice-screen");

        options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);


        service.HideCommandPromptWindow = true;

        var driver = new ChromeDriver(service, options);


        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

        return driver;
    }
}