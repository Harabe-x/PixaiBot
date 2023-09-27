using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;

namespace PixaiBot.Bussines_Logic;

public class AccountsManager : IAccountsManager
{
    public int AccountsCount => UpdateAccountManagerProperties();

    private string AccountsFilePath { get; }

    private readonly JsonReader _jsonReader;

    private readonly IBotStatisticsManager _botStatisticsManager;

    private readonly ILogger _logger;

    public AccountsManager(IBotStatisticsManager botStatisticsManager,ILogger logger)
    {
        _logger = logger;
        AccountsFilePath = InitialConfiguration.AccountsFilePath;
        _botStatisticsManager = botStatisticsManager;
        _jsonReader = new JsonReader();
    }

    public void AddAccount(UserAccount account)
    {

        if (!File.Exists(AccountsFilePath))
        {
            var accountsList = new List<UserAccount>();
            accountsList.Add(account);
            JsonWriter.WriteJson(accountsList, AccountsFilePath);
            _botStatisticsManager.IncreaseAccountsCount(1);
            UpdateAccountManagerProperties();
            _logger.Log("Added account ", _logger.ApplicationLogFilePath);

        return;
        }

        var accountList = _jsonReader.ReadAccountFile(AccountsFilePath);
        accountList.Add(account);
        JsonWriter.WriteJson(accountList, AccountsFilePath);
        _botStatisticsManager.IncreaseAccountsCount(1);
        _logger.Log("Added account", _logger.ApplicationLogFilePath);
        UpdateAccountManagerProperties();

    }

    public void RemoveAccount(IList<UserAccount> accountList, UserAccount userAccount)
    {
        if (!File.Exists(AccountsFilePath)) return;
        accountList.Remove(userAccount);
        JsonWriter.WriteJson(accountList, AccountsFilePath);
        _botStatisticsManager.IncreaseAccountsCount(-1);
        UpdateAccountManagerProperties();
        _logger.Log("Removed account", _logger.ApplicationLogFilePath);

    }

    public IEnumerable<UserAccount> GetAllAccounts()
    {
        _logger.Log("Reading account List", _logger.ApplicationLogFilePath);

        return File.Exists(AccountsFilePath)
            ? _jsonReader.ReadAccountFile(AccountsFilePath)
            : new List<UserAccount>();
    }

    public void AddManyAccounts()
    {
        var dialog = new OpenFileDialog()
        {
            Title = "Select File:",
            Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        };

        var result = dialog.ShowDialog();

        if (result == false) return;
         
        var importedUserAccounts = GetUserAccountsFromTxt(dialog.FileName);
        _logger.Log("Adding accounts in batch", _logger.ApplicationLogFilePath);

        foreach (var account in importedUserAccounts) AddAccount(account);
    }

    private int UpdateAccountManagerProperties()
    {
        return GetAllAccounts().Count();
    }

    private IEnumerable<UserAccount> GetUserAccountsFromTxt(string filePath)
    {
        var accountsList = File.ReadAllLines(filePath);
        var accounts = new List<UserAccount>();
        _logger.Log("Reading accounts from txt File", _logger.ApplicationLogFilePath);

        foreach (var account in accountsList)
        {
            var splittedLogin = account.Split(":");

            if (splittedLogin.Length != 2) continue;

            var userAccount = new UserAccount()
            {
                Email = splittedLogin[0],
                Password = splittedLogin[1]
            };

            accounts.Add(userAccount);
        }

        return accounts;
    }
}