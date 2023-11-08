using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.Data.Models;

namespace PixaiBot.Data.Interfaces;

public interface IAccountsManager
{
    public int AccountsCount { get; }

    public event EventHandler AccountsListChanged;

    public void AddAccount(UserAccount account);

    public void RemoveAccount(UserAccount account);

    public IEnumerable<UserAccount> GetAllAccounts();

    public void AddManyAccounts();

    public void EditAccount(UserAccount account,string newEmail,string newPassword);
}