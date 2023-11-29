using System;
using PixaiBot.Bussines_Logic.Driver_and_Browser_Management.WebNavigationCore.WebNavigationCoreException;
using PixaiBot.Data.Interfaces;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;


namespace PixaiBot.Bussines_Logic.Driver_and_Browser_Management.WebNavigationCore
{
    internal class PixaiDataReader : IPixaiDataReader
    {

        public string GetUsername(ISearchContext searchContext)
        {
            return GetWebElementText(searchContext, ".text-\\[32px\\]");
        }

        public string GetCreditsCount(ISearchContext searchContext)
        {
            return GetWebElementText(searchContext, ".font-bold > span");
        }

        public string GetEmailVerificationStatus(ISearchContext searchContext)
        {
            return GetWebElementText(searchContext, ".leading-6");
        }

        public string GetFollowersCount(ISearchContext searchContext)
        {
            return GetWebElementText(searchContext, ".gap-1:nth-child(2) > .font-bold");
        }

        public string GetFollowingCount(ISearchContext searchContext)
        {
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
    }
}
