using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace PixaiBot.Data.Interfaces;

internal interface IPixaiDataReader
{
    public string GetAccountId(ISearchContext searchContext);

    public string GetUsername(ISearchContext searchContext);

    public string GetCreditsCount(ISearchContext searchContext);

    public string GetEmailVerificationStatus(ISearchContext searchContext);

    public string GetFollowersCount(ISearchContext searchContext);

    public string GetFollowingCount(ISearchContext searchContext);
}