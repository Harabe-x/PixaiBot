using System;
using System.Net.Mail;
using PixaiBot.Data.Interfaces;

namespace PixaiBot.Bussines_Logic.Data_Handling;

public class DataValidator : IDataValidator
{
    #region Methods
    /// <summary>
    /// Validates if <paramref name="email"/> is valid,
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
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

    public bool IsPasswordValid(string password)
    {
        return !string.IsNullOrEmpty(password);
    }

    #endregion
}