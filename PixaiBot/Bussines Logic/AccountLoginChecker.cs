using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;

namespace PixaiBot.Bussines_Logic
{
    internal class AccountLoginChecker : IAccountLoginChecker
    {

        

        private ChromeDriver _driver;

        public bool CheckAccountLogin(UserAccount userAccount)
        {
            _driver = new ChromeDriver();

            _driver.Navigate().GoToUrl("https://pixai.art/login");
            IReadOnlyCollection<IWebElement> buttons = _driver.FindElements(By.XPath("//button[text()='Login with email'"));

            foreach (var button in buttons)
            {
                MessageBox.Show(button.Text);
            }
                                       
            return true;
        }

        public void CheckAllAccountsLogin(IList<UserAccount> userAccountsList)
        {
            throw new NotImplementedException();
        }
    }
}
