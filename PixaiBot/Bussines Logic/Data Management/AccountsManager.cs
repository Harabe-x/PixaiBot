using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using PixaiBot.Bussines_Logic.Data_Handling;
using PixaiBot.Data.Interfaces;
using PixaiBot.UI.Models;

namespace PixaiBot.Bussines_Logic.Data_Management;

public class AccountsManager : IAccountsManager
{
    #region Constructor

    public AccountsManager(IBotStatisticsManager botStatisticsManager, ILogger logger, IDataValidator dataValidator)
    {
        _dataValidator = dataValidator;
        _logger = logger;
        AccountsFilePath = InitialConfiguration.AccountsFilePath;
        _botStatisticsManager = botStatisticsManager;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Adds new account to the account list.
    /// </summary>
    /// <param name="account">Account to add.</param>
    public void AddAccount(UserAccount account)
    {
        var botStatistics = _botStatisticsManager.GetStatistics();

        if (!File.Exists(AccountsFilePath))
        {
            var accountsList = new List<UserAccount> { account };

            JsonWriter.WriteJson(accountsList, AccountsFilePath);

            botStatistics.AccountsCount = 1;

            _logger.Log("Added account ", _logger.ApplicationLogFilePath);

            AccountsListChanged?.Invoke(this, EventArgs.Empty);

            return;
        }

        var accountList = GetAllAccounts().ToList();

        accountList.Add(account);

        JsonWriter.WriteJson(accountList, AccountsFilePath);

        botStatistics.AccountsCount += 1;

        _logger.Log("Added account", _logger.ApplicationLogFilePath);

        _botStatisticsManager.SaveStatistics(botStatistics);

        AccountsListChanged?.Invoke(this, EventArgs.Empty);
    }


    /// <summary>
    /// Removes account from account file.
    /// </summary>
    /// <param name="userAccount">Account to remove.</param>
    public void RemoveAccount(UserAccount userAccount)
    {
        if (!File.Exists(AccountsFilePath)) return;

        var accountsList = GetAllAccounts().ToList();

        var accountToRemove =
            accountsList.FirstOrDefault(x => x.Email == userAccount.Email && x.Password == userAccount.Password);

        if (accountToRemove == null) return;

        accountsList.Remove(accountToRemove);

        var botStatistics = _botStatisticsManager.GetStatistics();

        botStatistics.AccountsCount -= 1;

        _botStatisticsManager.SaveStatistics(botStatistics);

        JsonWriter.WriteJson(accountsList, AccountsFilePath);

        AccountsListChanged?.Invoke(this, EventArgs.Empty);

        _logger.Log("Removed account", _logger.ApplicationLogFilePath);
    }


    /// <summary>
    ///  Reads and returns all accounts from accounts file.
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
    /// Opens File Dialog and extracts user accounts from txt file.
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

    /// <summary>
    /// Edits an existing account.
    /// </summary>
    /// <param name="account">Old account.</param>
    /// <param name="newEmail">New account email.</param>
    /// <param name="newPassword">New Account password.</param>
    public void EditAccount(UserAccount account, string newEmail, string newPassword)
    {
        if (newEmail == null || newPassword == null) return;

        if (!_dataValidator.IsEmailValid(newEmail) || !_dataValidator.IsPasswordValid(newPassword)) return;

        var newAccount = new UserAccount()
        {
            Email = newEmail,
            Password = newPassword
        };

        AddAccount(newAccount);

        RemoveAccount(account);

        _logger.Log("Edited account", _logger.ApplicationLogFilePath);
    }



    /// <summary>
    /// Extracts accounts from a text file.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
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

            var userAccount = new UserAccount
            {
                Email = splittedLogin[0],
                Password = splittedLogin[1]
            };

            accountsToWrite.Add(userAccount);
        }

        return accountsToWrite;
    }

    #endregion

    #region Fields

    private string AccountsFilePath { get; }

    public event EventHandler? AccountsListChanged;

    private readonly IBotStatisticsManager _botStatisticsManager;

    private readonly ILogger _logger;

    private readonly IDataValidator _dataValidator;

    #endregion
}