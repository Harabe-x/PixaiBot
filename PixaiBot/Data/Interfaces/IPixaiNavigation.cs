using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace PixaiBot.Data.Interfaces
{
    internal interface IPixaiNavigation
    {
        public void GoBack(IWebDriver searchContext);

        public void GoToRegistrationPage(ISearchContext searchContext);

        public void GoToLoginPage(ISearchContext searchContext);

        public void SendCredentialsToTextBoxes(ISearchContext searchContext, string email,string password);

        public void ClickOnRegisterButton(ISearchContext searchContext);

        public void ClickOnLoginButton(ISearchContext searchContext);

        public void GoToProfile(ISearchContext searchContext);

        public void GoToProfileSettings(ISearchContext searchContext);

        public void GoToCreditsTab(IWebDriver webDriver);
            
        public void ClickDropdownMenu(IWebDriver webDriver);

        public void GoToMyWorkTab(IWebDriver webDriver);

        public void NavigateToUrl(IWebDriver webDriver, string url);
    }
}
