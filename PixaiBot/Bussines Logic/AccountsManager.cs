using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;

namespace PixaiBot.Bussines_Logic
{
    public class AccountsManager : IAccountsManager
    {
        public int AccountsCount { get; private set; }

        public const string AccountsFilePath = "accounts.json";

        private readonly JsonReader _jsonReader;

        public AccountsManager()
        {
            _jsonReader = new JsonReader();
            UpdateAccountManagerProperties();
        }

        public void AddAccount(UserAccount account)
        {
            if (!File.Exists(AccountsFilePath))
            {
                var accountsList = new List<UserAccount>();
                accountsList.Add(account);
                _jsonReader.WriteAccountList(accountsList, AccountsFilePath);
                return;
            }

            var accountList = _jsonReader.ReadAccountFile(AccountsFilePath);
            accountList.Add(account);
            _jsonReader.WriteAccountList(accountList, AccountsFilePath);

            UpdateAccountManagerProperties();
        }

        public void RemoveAccount(IList<UserAccount> accountList, UserAccount userAccount)
        {
            if (!File.Exists(AccountsFilePath))
            {
                return;
            }

            accountList.Remove(userAccount);
            _jsonReader.WriteAccountList(accountList, AccountsFilePath);
          
            UpdateAccountManagerProperties();
        }

        public IEnumerable<UserAccount> GetAllAccounts()
        {
            return File.Exists(AccountsFilePath)
                ? _jsonReader.ReadAccountFile(AccountsFilePath)
                : new List<UserAccount>();
        }

        private void UpdateAccountManagerProperties()
        {
            AccountsCount = GetAllAccounts().Count();
        }
    }
}
