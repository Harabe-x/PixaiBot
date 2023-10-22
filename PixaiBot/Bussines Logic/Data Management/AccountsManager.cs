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


    private readonly IBotStatisticsManager _botStatisticsManager;

    private readonly ILogger _logger;

    private readonly IDataValidator _dataValidator;

    public AccountsManager(IBotStatisticsManager botStatisticsManager, ILogger logger,IDataValidator dataValidator)
    {
        _dataValidator = dataValidator;
        _logger = logger;
        AccountsFilePath = InitialConfiguration.AccountsFilePath;
        _botStatisticsManager = botStatisticsManager;
    }


    /// <summary>
    /// Add new account to accounts file
    /// </summary>
    /// <param name="account"></param>
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

        var accountList = JsonReader.ReadAccountFile(AccountsFilePath);
        accountList.Add(account);
        JsonWriter.WriteJson(accountList, AccountsFilePath);
        _botStatisticsManager.IncreaseAccountsCount(1);
        _logger.Log("Added account", _logger.ApplicationLogFilePath);
        UpdateAccountManagerProperties();
    }


    /// <summary>
    /// Removes account from accounts file
    /// </summary>
    /// <param name="accountList"></param>
    /// <param name="userAccount"></param>
    public void RemoveAccount(IList<UserAccount> accountList, UserAccount userAccount)
    {
        if (!File.Exists(AccountsFilePath)) return;
        accountList.Remove(userAccount);
        JsonWriter.WriteJson(accountList, AccountsFilePath);
        _botStatisticsManager.IncreaseAccountsCount(-1);
        UpdateAccountManagerProperties();
        _logger.Log("Removed account", _logger.ApplicationLogFilePath);
    }


    /// <summary>
    /// Returns all accounts from accounts file
    /// </summary>
    /// <returns></returns>
    public IEnumerable<UserAccount> GetAllAccounts()
    {
        _logger.Log("Reading account List", _logger.ApplicationLogFilePath);

        return File.Exists(AccountsFilePath)
            ? JsonReader.ReadAccountFile(AccountsFilePath)
            : new List<UserAccount>();
    }

    
    /// <summary>
    /// Opens File Dialog and extracts user accounts from txt file
    /// </summary>
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
        
        var accountsToWrite = new List<UserAccount>();
        _logger.Log("Reading accounts from txt File", _logger.ApplicationLogFilePath);

        foreach (var account in accountsList)
        {
            var splittedLogin = account.Split(":");

            if (splittedLogin.Length != 2) continue;

            if (string.IsNullOrEmpty(splittedLogin[0]) || string.IsNullOrEmpty(splittedLogin[1])) continue;

            if (_dataValidator.IsEmailValid(splittedLogin[0]) && _dataValidator.IsPasswordValid(splittedLogin[1])) continue;
          
            var userAccount = new UserAccount
            {
                Email = splittedLogin[0],
                Password = splittedLogin[1]
            };

            accountsToWrite.Add(userAccount);
        }

        return accountsToWrite;
    }
}