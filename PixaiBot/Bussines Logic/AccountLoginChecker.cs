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

        public bool CheckAccountLogin(UserAccount userAccount)
        {
            _driver = new ChromeDriver();

            var account = new UserAccount()
            {
                Email = "xgra577@gmail.com",
                Password = "Lenny231"
            };

            LoginModule.Login(_driver,account);

            return false;

        }

        public void CheckAllAccountsLogin(IList<UserAccount> userAccountsList)
        {
            throw new NotImplementedException();
        }
    }
}
