using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixaiBot.Data.Interfaces;

internal interface ILoginCredentialsMaker
{

    /// <summary>
    ///  Generates a random email address.
    /// </summary>
    /// <returns>Generated Email Address.</returns>
    public string GenerateEmail();


    /// <summary>
    ///  Generates a random email with temp-mail domain.
    /// </summary>
    /// <param name="tempMailApiKey"></param>
    /// <returns>Generated Email Address.</returns>
    public string GenerateEmail(string tempMailApiKey);


    /// <summary>
    /// Generates a strong password.
    /// </summary>
    /// <returns>Generated strong password</returns>
    public string GeneratePassword();
}