using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace PixaiBot.Business_Logic.Driver_and_Browser_Management;

public static class ChromeDriverFactory
{
    /// <summary>
    /// Creates a ChromeDriver with the needed settings to hide the process.
    /// </summary>
    /// <returns>Chrome Driver Instance</returns>
    public static ChromeDriver CreateDriver()
    {
        var options = new ChromeOptions();

        options.AddArgument("--window-position=-32000,-32000");

        var service = ChromeDriverService.CreateDefaultService();

        service.HideCommandPromptWindow = true;

        var driver = new ChromeDriver(service, options);

        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(MaxWaitTime);

        return driver;
    }

    /// <summary>
    ///  Creates a ChromeDriver with the needed settings to hide the process and run it headless.
    /// </summary>
    /// <returns>Chrome Driver Instance</returns>
    public static ChromeDriver CreateHeadlessDriver()
    {
        var options = new ChromeOptions();

        options.AddArgument("--window-position=-32000,-32000");

        options.AddArgument("--headless");

        var service = ChromeDriverService.CreateDefaultService();

        service.HideCommandPromptWindow = true;

        var driver = new ChromeDriver(service, options);

        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(MaxWaitTime);

        return driver;
    }



    /// <summary>
    /// Creates a ChromeDriver with the needed settings to hide the process.
    /// </summary>
    /// <param name="proxy">Proxy server that should be used with Chrome Driver</param>
    /// <returns></returns>
    public static ChromeDriver CreateDriver(string proxy)
    {
        var options = new ChromeOptions();

        options.AddArgument("--window-position=-32000,-32000");

        var proxyObject = new Proxy()
        {
            HttpProxy = proxy,
            FtpProxy = proxy,
            SocksProxy = proxy,
            SslProxy = proxy,
            Kind = ProxyKind.AutoDetect
        };

        options.Proxy = proxyObject;

        var service = ChromeDriverService.CreateDefaultService();

        service.HideCommandPromptWindow = true;

        var driver = new ChromeDriver(service, options);

        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(MaxWaitTime);

        return driver;
    }


    /// <summary>
    /// Returns ChromeDriver with default settings for debugging purposes.
    /// </summary>
    /// <returns></returns>
    public static ChromeDriver CreateDriverForDebug()
    {
        var driver = new ChromeDriver();

        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(MaxWaitTime);

        return driver;
    }

    /// <summary>
    /// Implicit Wait Time for Chrome Driver
    /// </summary>
    private const int MaxWaitTime = 5;
}