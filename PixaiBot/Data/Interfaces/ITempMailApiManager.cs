using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixaiBot.Data.Interfaces;

public interface ITempMailApiManager
{
    /// <summary>
    /// Occurs when the request to the API failed.
    /// </summary>
    event EventHandler<string> RequestFailed;

    /// <summary>
    ///  Gets the available domains from API.
    /// </summary>
    /// <param name="tempMailApiKey">Temp-mail api key</param>
    /// <returns>List of available domains </returns>
    string GetDomain(string tempMailApiKey);

    /// <summary>
    /// Gets Verification link from email.
    /// </summary>
    /// <param name="email">Email from which messages are to be downloaded</param>
    /// <param name="apiKey">Temp-mail api key</param>
    /// <returns>First link from email</returns>
    string GetVerificationLink(string email, string apiKey);
}