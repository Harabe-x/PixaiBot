using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using PixaiBot.Data.Interfaces;

namespace PixaiBot.Bussines_Logic.Driver_and_Browser_Management.WebNavigationCore
{
    internal class PixaiDataReader : IPixaiDataReader
    {
        public string GetUsername(ChromeDriver driver)
        {
            throw new NotImplementedException();
        }

        public string GetCreditsCount(ChromeDriver driver)
        {
            throw new NotImplementedException();
        }

        public string GetEmailVerificationStatus(ChromeDriver driver)
        {
            throw new NotImplementedException();
        }

        public string GetFollowersCount(ChromeDriver driver)
        {
            throw new NotImplementedException();
        }

        public string GetFollowingCount(ChromeDriver driver)
        {
            throw new NotImplementedException();
        }
    }
}
