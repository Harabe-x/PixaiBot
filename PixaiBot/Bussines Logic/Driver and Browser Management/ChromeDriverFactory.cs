using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;

namespace PixaiBot.Bussines_Logic
{
    public static class ChromeDriverFactory
    {
        public static ChromeDriver CreateDriver()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--window-position=-32000,-32000");

            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            return new ChromeDriver(service, options);
        }
    }

}
