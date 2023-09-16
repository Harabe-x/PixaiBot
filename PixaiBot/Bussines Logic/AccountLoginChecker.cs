using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
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
        private ChromeDriver? _driver;

        private const string _mainPageUrl = "https://pixai.art/";

        private const string _accountsFileName = @"C:\Users\xgra5\AppData\Roaming\PixaiAutoClaimer\accounts.json";

        public bool CheckAccountLogin(UserAccount userAccount)
        {
            _driver = new ChromeDriver();

            LoginModule.Login(_driver, userAccount);

            Thread.Sleep(1000);

            if (_driver.Url == _mainPageUrl )
            {
                _driver.Close();
                _driver.Dispose();
                return true;
            }

            _driver.Close();
            _driver.Dispose();
            return false; 
        }
        public int CheckAllAccountsLogin(IList<UserAccount> accountsList)
        {
            var validAccounts = accountsList.Where(CheckAccountLogin).ToList();

            JsonWriter.WriteJson(validAccounts, _accountsFileName);

            return validAccounts.Count;
        }
        
    }
}
