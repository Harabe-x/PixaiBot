using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace PixaiBot.Business_Logic.Driver_and_Browser_Management.Driver_Creation_Strategy
{
   public class HeadlessDriverCreationStrategy : IDriverCreationStrategy
    {
        public IWebDriver CreateDriver()
        {
            var options = new ChromeOptions();

            options.AddArgument("--headless");

            var service = ChromeDriverService.CreateDefaultService();

            service.HideCommandPromptWindow = true;

            var driver = new ChromeDriver(service, options);

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            return driver;
        }

        
    }
}
