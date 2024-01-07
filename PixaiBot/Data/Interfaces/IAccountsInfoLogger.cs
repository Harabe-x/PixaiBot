using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PixaiBot.Data.Models;

namespace PixaiBot.Data.Interfaces;

internal interface IAccountsInfoLogger
{
    /// <summary>
    ///  Clears the of the <see cref="StringBuilder"/> content.
    /// </summary>
    public void ClearStringBuilderContent();

    /// <summary>
    /// Logs information about the accounts in the <paramref name="userAccountsList"/>.
    /// </summary>
    /// <param name="userAccountsList">List of accounts to check.</param>
    /// <param name="settings">settings</param>
    /// <param name="cancellationToken">Cancellation token to cancel operation.</param>
    /// <returns>Accounts log</returns>
    public string StartLoggingAccountsInfo(IEnumerable<UserAccount> userAccountsList,
        IAccountInfoLoggerSettings settings, CancellationToken cancellationToken);
}