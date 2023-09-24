using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;

namespace PixaiBot.Bussines_Logic;

internal class AccountsStatisticsManager : IAccountsStatisticsManager
{
    private string AccountsStatisticsFilePath { get; }

    private AccountsStatistics _accountsStatistics;

    private JsonReader _jsonReader;

    private ILogger _logger;

    public AccountsStatisticsManager(ILogger logger)
    {
        _logger = logger;
        AccountsStatisticsFilePath = InitialConfiguration.StatisticsFilePath;
        _jsonReader = new JsonReader();
        RefreshStatistics();
    }

    public int AccountsNumber { get; private set; }

    public int AccountsWithUnclaimedCredits { get; private set; }

    public int AccountsWithClaimedCredits { get; private set; }

    public void RefreshStatistics()
    {
        _accountsStatistics = _jsonReader.ReadStatisticsFile(AccountsStatisticsFilePath);
        AccountsNumber = _accountsStatistics.AccountsCount;
        AccountsWithUnclaimedCredits = _accountsStatistics.AccountWithUnclaimedCredits;
        AccountsWithClaimedCredits = _accountsStatistics.AccountWithClaimedCredits;
    }

    public void IncrementAccountsNumber(int number)
    {
        _accountsStatistics.AccountsCount += number;
        JsonWriter.WriteJson(_accountsStatistics, AccountsStatisticsFilePath);
        RefreshStatistics();
        _logger.Log("Accounts count updated", _logger.ApplicationLogFilePath);
    }

    public void IncrementAccountsWithClaimedCreditsNumber(int number)
    {

        _accountsStatistics.AccountWithClaimedCredits += number;
        JsonWriter.WriteJson(_accountsStatistics, AccountsStatisticsFilePath);
        RefreshStatistics();
        _logger.Log("Accounts with claimed credits count updated", _logger.ApplicationLogFilePath);

    }

    public void IncrementAccountsWithUnclaimedCreditsNumber(int number)
    {
        _accountsStatistics.AccountWithUnclaimedCredits += number;
        JsonWriter.WriteJson(_accountsStatistics, AccountsStatisticsFilePath);
        RefreshStatistics();
        _logger.Log("Accounts with unclaimed credits count updated", _logger.ApplicationLogFilePath);

    }

    public void WriteStatisticsToFile()
    {
        JsonWriter.WriteJson(_accountsStatistics, AccountsStatisticsFilePath);
        _logger.Log("Writed statistics file ", _logger.ApplicationLogFilePath);
    }

    public void ResetNumberOfAccounts()
    {
        _accountsStatistics.AccountsCount = 0;
        _logger.Log("Account count rested", _logger.ApplicationLogFilePath);

    }
}