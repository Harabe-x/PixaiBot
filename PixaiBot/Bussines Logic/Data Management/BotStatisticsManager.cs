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

    public event EventHandler? StatisticsChanged;

    public BotStatisticsManager(ILogger logger)
    {
        _logger = logger;
        AccountsStatisticsFilePath = InitialConfiguration.StatisticsFilePath;
        _jsonReader = new JsonReader();
      _botStatistics =  _jsonReader.ReadStatisticsFile(AccountsStatisticsFilePath);
        InitializeData();
    }

    private void InitializeData()
    {
      BotVersion = _botStatistics.BotVersion;
      LastCreditClaimDateTime = _botStatistics.LastCreditClaimDateTime;
      AccountsNumber = _botStatistics.AccountsCount;
    }

    private int _accountsNumber;

    public int AccountsNumber
    {
        get => _accountsNumber;
        private set
        {
            if (_accountsNumber == value) return;
            _accountsNumber = value;
            _botStatistics.AccountsCount = value;
             SaveStatistics();
             StatisticsChanged?.Invoke(this, EventArgs.Empty);

        }
    }

    private DateTime _lastCreditClaimDateTime;

    public DateTime LastCreditClaimDateTime
    {
        get => _lastCreditClaimDateTime;
        private set
        {
            if (_lastCreditClaimDateTime == value) return;
            _lastCreditClaimDateTime = value;
            _botStatistics.LastCreditClaimDateTime = value;
            SaveStatistics();
            StatisticsChanged?.Invoke(this, EventArgs.Empty);

        }
    }

    private string _botVersion;
    
    public string BotVersion 
    { get => _botVersion;
        private set
        {
            if (_botVersion == value) return;
            _botStatistics.BotVersion = value;
            _botVersion = value;
            SaveStatistics();
            StatisticsChanged?.Invoke(this, EventArgs.Empty);
        }
    }


    public void IncreaseAccountsCount(int number)
    {
        AccountsNumber += number;

        _logger.Log("Accounts count updated", _logger.ApplicationLogFilePath);
    }

    public void SetClaimDateTime(DateTime creditClaimDate)
    {

        LastCreditClaimDateTime = creditClaimDate ;
    }


    public void SaveStatistics()
    {
        JsonWriter.WriteJson(_botStatistics, AccountsStatisticsFilePath);
        
        _logger.Log("Writed statistics file ", _logger.ApplicationLogFilePath);
    }

    public void ResetNumberOfAccounts()
    {
        AccountsNumber = 0;

    }
}