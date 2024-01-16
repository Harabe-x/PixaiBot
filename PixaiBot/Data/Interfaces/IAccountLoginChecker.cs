using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PixaiBot.Business_Logic.Driver_and_Browser_Management.Driver_Creation_Strategy;
using PixaiBot.UI.Models;

namespace PixaiBot.Data.Interfaces;

public interface IAccountLoginChecker
{
    /// <summary>
    ///  Occurs when login to the account is successful.
    /// </summary>
    event EventHandler<UserAccount> ValidAccountLogin;

    /// <summary>
    /// Checks the credentials of a single account.
    /// </summary>
    /// <param name="userAccount">Checked account.</param>
    /// <param name="driverCreationStrategy">Driver creation strategy.</param>
    /// <returns>Boolean indicating the login status.</returns>
    bool CheckAccountLogin(UserAccount userAccount, IDriverCreationStrategy driverCreationStrategy);

    /// <summary>
    /// Checks the correctness of login details of accounts on the list.
    /// </summary>
    /// <param name="accountsList">Accounts to be checked.</param>
    /// <param name="token">Cancellation token to cancel operation.</param>
    /// <param name="driverCreationStrategy">Driver creation strategy.</param>
    /// <returns>List of valid <see cref="UserAccount"/>.</returns>
    IEnumerable<UserAccount> CheckAllAccountsLogin(IEnumerable<UserAccount> accountsList, IDriverCreationStrategy driverCreationStrategy,CancellationToken token);
}