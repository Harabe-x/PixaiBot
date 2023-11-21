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
    /// Creates a ChromeDriver with the needed settings to hide the process.
    /// </summary>
    /// <returns>Chrome Driver Instance</returns>
    public static ChromeDriver CreateDriver()
    {
        var options = new ChromeOptions();
        options.AddArgument("--window-position=-32000,-32000");
        options.Proxy = new Proxy();
        var service = ChromeDriverService.CreateDefaultService();
        service.HideCommandPromptWindow = true;

        return new ChromeDriver(service, options);
    }


    /// <summary>
    /// Creates a ChromeDriver with the needed settings to hide the process.
    /// </summary>
    /// <param name="proxy">Proxy server that should be used with Chrome Driver</param>
    /// <returns></returns>
    public static ChromeDriver CreateDriver(string proxy)
    {
        var options = new ChromeOptions();
        //options.AddArgument("--window-position=-32000,-32000");

        var proxyObject = new Proxy()
        {
            HttpProxy = proxy,
            Kind = ProxyKind.Manual,
        };
        
        options.Proxy = proxyObject;

        options.AddUserProfilePreference("webrtc.ip_handling_policy", "disable_non_proxied_udp");

        options.AddArgument("--ignore-certificate-errors");


        var service = ChromeDriverService.CreateDefaultService();
        service.HideCommandPromptWindow = true;

        return new ChromeDriver(service, options);
    }


    /// <summary>
    /// Returns ChromeDriver with default settings for debugging purposes.
    /// </summary>
    /// <returns></returns>
    public static ChromeDriver CreateDriverForDebug()
    {
        return new ChromeDriver();
    }


    }