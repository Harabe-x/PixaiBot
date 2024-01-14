using System;
using System.Net.Mail;
using PixaiBot.Data.Interfaces;

namespace PixaiBot.Business_Logic.Data_Handling;

public class DataValidator : IDataValidator
{
    #region Methods

    /// <summary>
    /// Checks if <paramref name="email"/> is valid,
    /// </summary>
    /// <param name="email"></param>
    /// <returns>If <paramref name="email"/> is valid returns True; otherwise returns False</returns>
    public bool IsEmailValid(string email)
    {
        try
        {
            var mail = new MailAddress(email);
        }
        catch (ArgumentNullException)
        {
            return false;
        }
        catch (ArgumentException)
        {
            return false;
        }
        catch (FormatException)
        {
            return false;
        }

        return true;
    }


    /// <summary>
    /// Checks if <paramref name="password"/> is valid,
    /// </summary>
    /// <param name="password"></param>
    /// <returns>If <paramref name="password"/> is valid returns True; otherwise returns False</returns>
    public bool IsPasswordValid(string password)
    {
        return !string.IsNullOrEmpty(password);
    }

    #endregion
}