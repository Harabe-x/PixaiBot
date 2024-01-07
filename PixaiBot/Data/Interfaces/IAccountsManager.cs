using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.UI.Models;

namespace PixaiBot.Data.Interfaces;

public interface IAccountsManager
{

    /// <summary>
    ///  Occurs when the accounts list is changed.
    /// </summary>
    public event EventHandler AccountsListChanged;

    /// <summary>
    /// Adds an account to the accounts list.
    /// </summary>
    /// <param name="account">Account to add</param>
    public void AddAccount(UserAccount account);

    /// <summary>
    /// Removes an account from the accounts list.
    /// </summary>
    /// <param name="account">Account to remove</param>
    public void RemoveAccount(UserAccount account);

    /// <summary>
    /// Read all accounts from the accounts file.
    /// </summary>
    /// <returns>List of <see cref="UserAccount"/></returns>
    public IEnumerable<UserAccount> GetAllAccounts();

    /// <summary>
    /// Opens file dialog to select accounts file. 
    /// </summary>
    public void AddManyAccounts();


    /// <summary>
    /// Edits an account.
    /// </summary>
    /// <param name="account">Account to edit</param>
    /// <param name="newEmail">New account email</param>
    /// <param name="newPassword">New account password</param>
    public void EditAccount(UserAccount account, string newEmail, string newPassword);
}