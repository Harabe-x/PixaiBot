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

    IEnumerable<string> GetDomains(string tempMailApiKey);

    string GetVerificationLink(string email, string apiKey);
}