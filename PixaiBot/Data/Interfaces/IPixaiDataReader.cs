using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;

namespace PixaiBot.Data.Interfaces
{
    internal interface IPixaiDataReader
    {
        public string GetUsername(ChromeDriver driver);

        public string GetCreditsCount(ChromeDriver driver);

        public string GetEmailVerificationStatus(ChromeDriver driver);

        public string GetFollowersCount(ChromeDriver driver);

        public string GetFollowingCount(ChromeDriver driver);
    }
}
