using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.Data.Interfaces;
using System.Net.Mail;
using Microsoft.IdentityModel.Tokens;

namespace PixaiBot.Bussines_Logic;

public class DataValidator : IDataValidator
{
    #region Methods 
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