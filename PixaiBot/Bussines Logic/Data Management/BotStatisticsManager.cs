using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixaiBot.Data.Interfaces;
using PixaiBot.Data.Models;

namespace PixaiBot.Bussines_Logic;


//TODO: Refactor    

public class BotStatisticsManager : IBotStatisticsManager
{
    private string AccountsStatisticsFilePath { get; }

    private BotStatistics _botStatistics;

    private ILogger _logger;

    public event EventHandler? StatisticsChanged;

    private readonly ITcpServerConnector _tcpServerConnector;

    public BotStatisticsManager(ILogger logger,ITcpServerConnector tcpServerConnector )
    {
        _tcpServerConnector = tcpServerConnector;
        _logger = logger;
        AccountsStatisticsFilePath = InitialConfiguration.StatisticsFilePath;
      _botStatistics =  JsonReader.ReadStatisticsFile(AccountsStatisticsFilePath);
        InitializeData();
    }

    private void InitializeData()
    {
      BotVersion = InitialConfiguration.BotVersion;
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
    


    /// <summary>
    /// Increases <see cref="AccountsNumber"/> by <paramref name="number"/>
    /// </summary>
    /// <param name="number"></param>
    public void IncreaseAccountsCount(int number)
    {
        AccountsNumber += number;
        _tcpServerConnector.SendMessage("cIncreasing account count");

        _logger.Log("Accounts count updated", _logger.ApplicationLogFilePath);
    }


    /// <summary>
    /// Sets <see cref="LastCreditClaimDateTime"/> to current date time
    /// </summary>
    /// <param name="creditClaimDate"></param>
    public void SetClaimDateTime(DateTime creditClaimDate)
    {
        _tcpServerConnector.SendMessage("cSetting Last Claim Date");
        LastCreditClaimDateTime = creditClaimDate ;
    }

    /// <summary>
    /// Writes statistics to file
    /// </summary>
    public void SaveStatistics()
    {
        JsonWriter.WriteJson(_botStatistics, AccountsStatisticsFilePath);
        _tcpServerConnector.SendMessage("gWritten statistics file");
        _logger.Log("Written statistics file ", _logger.ApplicationLogFilePath);
    }

    public void ResetNumberOfAccounts()
    {
        AccountsNumber = 0;

    }
}