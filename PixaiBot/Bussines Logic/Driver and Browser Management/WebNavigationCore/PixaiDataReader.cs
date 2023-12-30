using System;
using PixaiBot.Bussines_Logic.Driver_and_Browser_Management.WebNavigationCore.WebNavigationCoreException;
using PixaiBot.Data.Interfaces;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;


namespace PixaiBot.Bussines_Logic.Driver_and_Browser_Management.WebNavigationCore
{
    internal class PixaiDataReader : IPixaiDataReader
    {

        public PixaiDataReader(ITcpServerConnector serverConnector)
        {
           _serverConnector = serverConnector;
        }

        private readonly ITcpServerConnector _serverConnector;

        public string GetAccountId(ISearchContext searchContext)
        {
            _serverConnector.SendMessage("mAccount id");

            return GetWebElementText(searchContext, ".font-bold");
        }

        public string GetUsername(ISearchContext searchContext)
        {
            _serverConnector.SendMessage("mGetting username");
            return GetWebElementText(searchContext, ".text-\\[32px\\]");
        }

        public string GetCreditsCount(ISearchContext searchContext)
        {
            _serverConnector.SendMessage("mGetting credits count");
            return GetWebElementText(searchContext, ".font-bold > span");
        }

        public string GetEmailVerificationStatus(ISearchContext searchContext)
        {
            _serverConnector.SendMessage("mGetting email verification status");

            return GetWebElementText(searchContext, ".leading-6");
        }

        public string GetFollowersCount(ISearchContext searchContext)
        {
            _serverConnector.SendMessage("mGetting followers count");
            return GetWebElementText(searchContext, ".gap-1:nth-child(2) > .font-bold");
        }

        public string GetFollowingCount(ISearchContext searchContext)
        {
            _serverConnector.SendMessage("mGetting following count");
            return GetWebElementText(searchContext, ".gap-2 > .flex:nth-child(1) > .font-bold");
        }
        private static string GetWebElementText(ISearchContext searchContext, string cssSelector)
        {
            IWebElement element;
            try
            {
                element = searchContext.FindElement(By.CssSelector(cssSelector));
            }
            catch (Exception e)
            {
                throw new ChromeDriverException("Chrome searchContext error occured while trying to get element text", e);
            }
            if (string.IsNullOrEmpty(element.Text)) { throw new EmptyTextException("There is no text in this element"); }

            return element.Text;
        }

        private static string GetWebElementAttribute(ISearchContext searchContext, string cssSelector,
            string attributeName)
        {
            IWebElement element;
            try
            {
                element = searchContext.FindElement(By.CssSelector(cssSelector));
            }
            catch (Exception e)
            {
                throw new ChromeDriverException("Chrome searchContext error occured while trying to get element text", e);
            }
            if (string.IsNullOrEmpty(element.GetAttribute(attributeName))) { throw new EmptyTextException("There is no text in this element"); }

            return element.Text;
        }
    }
}
