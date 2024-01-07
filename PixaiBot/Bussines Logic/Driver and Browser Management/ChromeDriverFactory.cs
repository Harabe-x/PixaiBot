using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace PixaiBot.Bussines_Logic;

public static class ChromeDriverFactory
{
    /// <summary>
    /// Creates a ChromeDriver with the needed model to hide the process.
    /// </summary>
    /// <returns>Chrome Driver Instance</returns>
    public static ChromeDriver CreateDriver()
    {
        var options = new ChromeOptions();

        options.AddArgument("--window-position=-32000,-32000");

        options.Proxy = new Proxy();

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
            Kind = ProxyKind.Manual
        };

        options.Proxy = proxyObject;

        options.AddArgument("--ignore-certificate-errors");

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