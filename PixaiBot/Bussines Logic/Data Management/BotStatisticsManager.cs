using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;

namespace PixaiBot.Bussines_Logic;

public class BotStatisticsManager : IBotStatisticsManager
{

    private string AccountsStatisticsFilePath { get; }

    private BotStatistics _botStatistics;

    private JsonReader _jsonReader;

    private ILogger _logger;

    public BotStatisticsManager(ILogger logger)
    {
        _logger = logger;
        AccountsStatisticsFilePath = InitialConfiguration.StatisticsFilePath;
        _jsonReader = new JsonReader();
        RefreshStatistics();
        SetApplicationVersion();
    }

    public int AccountsNumber { get; private set; }

    public DateTime LastCreditClaimDateTime { get; private set; }

    public string BotVersion { get; private set; }

    public void RefreshStatistics()
    {
        _botStatistics = _jsonReader.ReadStatisticsFile(AccountsStatisticsFilePath);
        AccountsNumber = _botStatistics.AccountsCount;
        LastCreditClaimDateTime = _botStatistics.LastCreditClaimDateTime;
        BotVersion = _botStatistics.BotVersion;

    }

    public void IncreaseAccountsCount(int number)
    {
        _botStatistics.AccountsCount += number;
        SaveStatistics();
        RefreshStatistics();
        _logger.Log("Accounts count updated", _logger.ApplicationLogFilePath);
    }

    public void SetClaimDateTime(DateTime creditClaimDate)
    {
        _botStatistics.LastCreditClaimDateTime = creditClaimDate;
        SaveStatistics();
    }


    public void SetApplicationVersion()
    {
        BotVersion = InitialConfiguration.BotVersion;
        _botStatistics.BotVersion = InitialConfiguration.BotVersion;
        SaveStatistics();
        RefreshStatistics();
    }

    public void SaveStatistics()
    {
        JsonWriter.WriteJson(_botStatistics, AccountsStatisticsFilePath);
        _logger.Log("Writed statistics file ", _logger.ApplicationLogFilePath);
    }

    public void ResetNumberOfAccounts()
    {
        _botStatistics.AccountsCount = 0;
        _logger.Log("Account count rested", _logger.ApplicationLogFilePath);

    }
}