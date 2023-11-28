using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;

namespace PixaiBot.Data.Interfaces
{
    internal interface IPixaiNavigation
    {
        public void GoBack(ChromeDriver driver);

        public void GoToRegistrationPage(ChromeDriver driver);

        public void GoToLoginPage(ChromeDriver driver);

        public void SendCredentialsToTextBoxes(ChromeDriver driver, string email,string password);

        public void ClickOnRegisterButton(ChromeDriver driver);

        public void ClickOnLoginButton(ChromeDriver driver);

        public void GoToProfile(ChromeDriver driver);

        public void GoToProfileSettings(ChromeDriver driver);

        public void GoToCreditsTab(ChromeDriver driver);

        public void ClickDropdownMenu(ChromeDriver driver);

        public void GoToMyWorkTab(ChromeDriver driver);

        public void NavigateToUrl(ChromeDriver driver, string url);
    }
}
