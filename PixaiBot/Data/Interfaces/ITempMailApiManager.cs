using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixaiBot.Data.Interfaces;

public interface ITempMailApiManager
{
    event EventHandler<string> RequestFailed;

    string GetEmail(string apiKey);

    IEnumerable<string> GetDomains(string tempMailApiKey);

    string GetVerificationLink(string email, string apiKey);
}