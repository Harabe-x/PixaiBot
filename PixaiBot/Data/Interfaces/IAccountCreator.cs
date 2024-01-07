using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PixaiBot.UI.Models;

namespace PixaiBot.Data.Interfaces;

internal interface IAccountCreator
{
    /// <summary>
    ///   Occurs when an account is created.
    /// </summary>
    event EventHandler<UserAccount> AccountCreated;

    /// <summary>
    ///  Occurs when an error occurs.
    /// </summary>
    event EventHandler<string> ErrorOccurred;


    /// <summary>
    ///  Creates accounts on https://pixai.art.
    /// </summary>
    /// <param name="amount">Number of accounts to create.</param>
    /// <param name="tempMailApiKey">TempMail api key.</param>
    /// <param name="shouldUseProxy">Determines whether the bot should use a proxy when creating an accounts.</param>
    /// <param name="shouldVerifyEmail">Determines whether the bot should verify email address.</param>
    /// <param name="token">cancellation token to cancel account creation process.</param>
    void CreateAccounts(int amount, string tempMailApiKey, bool shouldUseProxy, bool shouldVerifyEmail,
        CancellationToken token);
}