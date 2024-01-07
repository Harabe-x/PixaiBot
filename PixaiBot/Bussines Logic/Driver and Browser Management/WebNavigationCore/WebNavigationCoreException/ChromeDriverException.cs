using System;

namespace PixaiBot.Bussines_Logic.Driver_and_Browser_Management.WebNavigationCore.WebNavigationCoreException;

internal class ChromeDriverException : Exception
{
    public ChromeDriverException()
    {
    }

    public ChromeDriverException(string message) : base(message)
    {
    }

    public ChromeDriverException(string message, Exception innerException) : base(message, innerException)
    {
    }
}