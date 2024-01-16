using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace PixaiBot.Business_Logic.Driver_and_Browser_Management.Driver_Creation_Strategy
{
    class DebugDriverCreationStrategy : IDriverCreationStrategy
    {
        public IWebDriver CreateDriver()
        {

            var driver = new ChromeDriver();

            var service =ChromeDriverService.CreateDefaultService();

            service.HideCommandPromptWindow = true;

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            return driver;
        }
    }
}
