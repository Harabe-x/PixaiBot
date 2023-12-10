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

        public void ClickResendEmailVerificationLinkButton(ISearchContext searchContext);

        public void GoBack(IWebDriver searchContext);

        public void NavigateRegistrationPage(ISearchContext searchContext);

        public void GoToLoginPage(ISearchContext searchContext);

        public void SendLoginCredentialsToTextBoxes(ISearchContext searchContext, string email,string password);

        public void ClickOnRegisterButton(ISearchContext searchContext);

        public void ClickOnLoginButton(ISearchContext searchContext);

        public void NavigateToProfile(ISearchContext searchContext);

        public void NavigateToProfileSettings(ISearchContext searchContext);

        public void GoToCreditsTab(IWebDriver webDriver);
            
        public void ClickDropdownMenu(IWebDriver webDriver);

        public void NavigateToMyWorkTab(IWebDriver webDriver);

        public void NavigateToUrl(IWebDriver webDriver, string url);

        public void ClickClaimCreditButton(ISearchContext searchContext);
    }
}
