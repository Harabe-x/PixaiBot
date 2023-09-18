﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PixaiBot.Data.Models;

namespace PixaiBot.Bussines_Logic
{
    public abstract class LoginModule
    {
        private const string LoginUrl = "https://pixai.art/login";

        private const int StartWaitTime = 1000;

        protected static void Login(ChromeDriver driver,UserAccount userAccount)
        {

            driver.Navigate().GoToUrl(LoginUrl);
            
            Thread.Sleep(StartWaitTime);

            IReadOnlyCollection<IWebElement> buttons = driver.FindElements(By.TagName("button"));

            buttons.FirstOrDefault(x => x.Text == "Log in with email")?.Click();

            IReadOnlyCollection<IWebElement> textInputs = driver.FindElements(By.TagName("input"));

            if (textInputs.Count == 0)
                return;

            textInputs.ElementAt(0).Click();
            textInputs.ElementAt(0).SendKeys(userAccount.Email);

            textInputs.ElementAt(1).Click();
            textInputs.ElementAt(1).SendKeys(userAccount.Password);
            
            buttons = driver.FindElements(By.TagName("button"));

            buttons.FirstOrDefault(x => x.Text == "Login")?.Click();

        }
    }
}