using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;

namespace PixaiBot.Bussines_Logic;

public static class LoginModule
{
    private const string LoginUrl = "https://pixai.art/login";

    private const int StartWaitTime = 1000;


    /// <summary>
    /// Logs user to pixai.art
    /// <em>This method is used by CreditClaimer and AccountChecker</em>
    /// </summary>
    /// <param name="driver"></param>
    /// <param name="userAccount"></param>
    /// <param name="logger"></param>
    public static void Login(ChromeDriver driver, UserAccount userAccount, ILogger logger)
    {
        logger.Log("=====Launched Chrome Driver=====", logger.CreditClaimerLogFilePath);

        driver.Navigate().GoToUrl(LoginUrl);

        Thread.Sleep(StartWaitTime);

        logger.Log($"Logging in with {userAccount.Email}", logger.CreditClaimerLogFilePath);

        IReadOnlyCollection<IWebElement> buttons = driver.FindElements(By.TagName("button"));

        buttons.FirstOrDefault(x => x.Text == "Log in with email")?.Click();

        logger.Log($"Button Clicked", logger.CreditClaimerLogFilePath);


        logger.Log($"Finding TextBoxes {userAccount.Email}", logger.CreditClaimerLogFilePath);

        IReadOnlyCollection<IWebElement> textInputs = driver.FindElements(By.TagName("input"));

        if (textInputs.Count == 0)
        {
            logger.Log($"Textboxes not found", logger.CreditClaimerLogFilePath);

            return;
        }

        logger.Log($"Sending user credentials  to textboxes", logger.CreditClaimerLogFilePath);

        textInputs.ElementAt(0).Click();
        textInputs.ElementAt(0).SendKeys(userAccount.Email);

        textInputs.ElementAt(1).Click();
        textInputs.ElementAt(1).SendKeys(userAccount.Password);


        buttons = driver.FindElements(By.TagName("button"));

        if (buttons.FirstOrDefault(x => x.Text == "Login") == null)
        {
            logger.Log($"Login Button not found", logger.CreditClaimerLogFilePath);
            return;
        }

        buttons.FirstOrDefault(x => x.Text == "Login")?.Click();

        logger.Log($"Login Button Clicked", logger.CreditClaimerLogFilePath);
    }
}