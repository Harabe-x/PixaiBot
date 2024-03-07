using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PixaiBot.Data.Interfaces;

namespace PixaiBot.Business_Logic.Driver_and_Browser_Management.Driver_Creation_Strategy
{
    class ProxyDriverCreationStrategy : IDriverCreationStrategy
    {

        public ProxyDriverCreationStrategy(IProxyManager proxyManager)
        {
            _proxyManger = proxyManager;
        }

        public IWebDriver CreateDriver()
        {

            var proxy = _proxyManger.GetRandomProxy();

            

            var options = new ChromeOptions();

            options.AddArguments("--window-position=-32000,-32000", "--disable-crash-reporter", "--disable-gpu", "--disable-crash-reporter");
            options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);

            if (!string.IsNullOrEmpty(proxy))
            {
                var proxyObject = new Proxy()
                {
                    HttpProxy = proxy,
                    FtpProxy = proxy,
                    SocksProxy = proxy,
                    SslProxy = proxy,
                    Kind = ProxyKind.AutoDetect
                };

                options.Proxy = proxyObject;
            }

            var service = ChromeDriverService.CreateDefaultService();

            service.HideCommandPromptWindow = true;
            
            var driver = new ChromeDriver(service, options);

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            return driver;
        }


        private readonly IProxyManager _proxyManger;
    }
}
