using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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