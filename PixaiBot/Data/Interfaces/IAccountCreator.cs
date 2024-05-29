using System;
using System.Threading;
using PixaiBot.Business_Logic.Driver_and_Browser_Management.Driver_Creation_Strategy;
using PixaiBot.UI.Models;

namespace PixaiBot.Data.Interfaces;

internal interface IAccountCreator
{
    /// <summary>
    ///     Occurs when an account is created.
    /// </summary>
    event EventHandler<UserAccount> AccountCreated;

    /// <summary>
    ///     Occurs when an error occurs.
    /// </summary>
    event EventHandler<string> ErrorOccurred;


    /// <summary>
    ///     Creates accounts on https://pixai.art.
    /// </summary>
    /// <param name="amount">Number of accounts to create.</param>
    /// <param name="tempMailApiKey">TempMail api key.</param>
    /// <param name="shouldVerifyEmail">Determines whether the bot should verify email address.</param>
    /// <param name="interval">interval between the creation of accounts </param>
    /// <param name="token">cancellation token to cancel account creation process.</param>
    /// <param name="driverCreationStrategy">Driver creation strategy.</param>
    void CreateAccounts(int amount, string tempMailApiKey, bool shouldVerifyEmail,
        IDriverCreationStrategy driverCreationStrategy
        , TimeSpan interval, CancellationToken token);
}