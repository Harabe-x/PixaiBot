using System;

namespace PixaiBot.Business_Logic.Driver_and_Browser_Management.WebNavigationCore.WebNavigationCoreException;

internal class EmptyTextException : Exception
{
    public EmptyTextException()
    {
    }

    public EmptyTextException(string message) : base(message)
    {
    }

    public EmptyTextException(string message, Exception innerException) : base(message, innerException)
    {
    }
}