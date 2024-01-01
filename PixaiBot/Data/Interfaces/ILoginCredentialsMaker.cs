using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixaiBot.Data.Interfaces;

internal interface ILoginCredentialsMaker
{
    public string GenerateEmail();

    public string GenerateEmail(string tempMailApiKey);

    public string GeneratePassword();
}