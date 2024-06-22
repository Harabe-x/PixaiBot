using OpenQA.Selenium;

namespace PixaiBot.Business_Logic.Driver_and_Browser_Management.Driver_Creation_Strategy;

public interface IDriverCreationStrategy
{
    IWebDriver CreateDriver();
}