using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace PixaiBot.Business_Logic.Driver_and_Browser_Management.Driver_Creation_Strategy
{
    public interface IDriverCreationStrategy
    {
        IWebDriver CreateDriver();
    }
}
