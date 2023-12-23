using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PixaiBot.Data.Models;

namespace PixaiBot.Data.Interfaces
{
    internal interface IAccountCreator
    {
        event EventHandler<UserAccount> AccountCreated;

        event EventHandler<string> ErrorOccurred;

        void CreateAccounts(int amount, string tempMailApiKey, bool shouldUseProxy,bool shouldVerifyEmail,CancellationToken token);

    }
}
