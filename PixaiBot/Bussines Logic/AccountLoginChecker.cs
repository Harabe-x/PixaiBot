using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;

namespace PixaiBot.Bussines_Logic
{
    internal class AccountLoginChecker : LoginModule , IAccountLoginChecker
    {
        private ChromeDriver _driver;

        private const string _mainPageUrl = "https://www.pixai.art";

        public bool CheckAccountLogin(UserAccount userAccount)
        {
            _driver = new ChromeDriver();

            LoginModule.Login(_driver, userAccount);


            if (_driver.Url == _mainPageUrl )
            {
                _driver.Close();
                return true;
            }

            _driver.Close();
            return false; 
        }

        public void CheckAllAccountsLogin(IList<UserAccount> accountsList)
        {
            foreach (var account in accountsList)
            {
                CheckAccountLogin(account);
            }
        }
    }
}
