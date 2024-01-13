using System;

namespace PixaiBot.Business_Logic.Driver_and_Browser_Management.WebNavigationCore.WebNavigationCoreException;

internal class InvalidPageContentException : Exception
{
    public InvalidPageContentException()
    {
    }

    public InvalidPageContentException(string message) : base(message)
    {
    }

    public InvalidPageContentException(string message, Exception innerException) : base(message, innerException)
    {
    }
}